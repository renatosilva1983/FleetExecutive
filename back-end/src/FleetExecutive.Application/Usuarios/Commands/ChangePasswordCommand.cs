using FleetExecutive.Application.Common.Exceptions;
using FleetExecutive.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Usuarios.Commands;

/// <summary>
/// Usuário autenticado troca a própria senha (Estrutura/07-autenticacao-seguranca-rbac.md).
/// UserId nunca vem do request — sempre de ICurrentUser (JWT), ver "Anti-tampering".
/// </summary>
public record ChangePasswordCommand(string SenhaAtual, string NovaSenha) : IRequest;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.SenhaAtual).NotEmpty();
        RuleFor(x => x.NovaSenha).NotEmpty().MinimumLength(8)
            .WithMessage("A senha precisa ter no mínimo 8 caracteres.");
    }
}

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUser _currentUser;

    public ChangePasswordCommandHandler(IApplicationDbContext db, IPasswordHasher passwordHasher,
        ICurrentUser currentUser)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _currentUser = currentUser;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is null)
        {
            throw new AuthenticationException();
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == _currentUser.UserId, cancellationToken)
            ?? throw new AuthenticationException();

        if (!_passwordHasher.Verify(request.SenhaAtual, user.SenhaHash))
        {
            throw new AuthenticationException();
        }

        user.AtualizarSenha(_passwordHasher.Hash(request.NovaSenha));

        var outrasSessoes = await _db.UserSessions
            .Where(s => s.UserId == user.Id && s.RevogadoEm == null)
            .ToListAsync(cancellationToken);
        foreach (var sessao in outrasSessoes)
        {
            sessao.Revogar();
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
