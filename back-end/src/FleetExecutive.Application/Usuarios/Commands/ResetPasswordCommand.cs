using FleetExecutive.Application.Common;
using FleetExecutive.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Usuarios.Commands;

public record ResetPasswordCommand(string Token, string NovaSenha) : IRequest;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.NovaSenha).NotEmpty().MinimumLength(8)
            .WithMessage("A senha precisa ter no mínimo 8 caracteres.");
    }
}

/// <summary>
/// Ver Estrutura/07-autenticacao-seguranca-rbac.md — ao concluir, invalida todas as sessões
/// (refresh tokens) ativas do usuário, já que a senha mudou.
/// </summary>
public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordCommandHandler(IApplicationDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = TokenHasher.Hash(request.Token);
        var resetToken = await _db.PasswordResetTokens
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

        if (resetToken is null || !resetToken.EstaValido)
        {
            // Mensagem genérica — não revela se o token existe, já expirou ou já foi usado.
            throw new InvalidOperationException("Token de redefinição inválido ou expirado.");
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == resetToken.UserId, cancellationToken)
            ?? throw new InvalidOperationException("Token de redefinição inválido ou expirado.");

        user.AtualizarSenha(_passwordHasher.Hash(request.NovaSenha));
        resetToken.MarcarUsado();

        var sessoesAtivas = await _db.UserSessions
            .Where(s => s.UserId == user.Id && s.RevogadoEm == null)
            .ToListAsync(cancellationToken);
        foreach (var sessao in sessoesAtivas)
        {
            sessao.Revogar();
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
