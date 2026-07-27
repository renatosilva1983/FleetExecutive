using FleetExecutive.Application.Common;
using FleetExecutive.Application.Common.Exceptions;
using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Usuarios.Dtos;
using FleetExecutive.Domain.Usuarios;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Usuarios.Commands;

public record LoginCommand(string Email, string Senha, bool ManterConectado) : IRequest<LoginResult>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Senha).NotEmpty();
    }
}

/// <summary>
/// Resposta de erro sempre genérica ("e-mail ou senha inválidos"), nunca revelando se o e-mail
/// existe — ver Estrutura/07-autenticacao-seguranca-rbac.md, anti user-enumeration. O rate
/// limiting por IP/e-mail é aplicado na camada Api (ver Program.cs), não aqui.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private static readonly TimeSpan RefreshTokenValidade = TimeSpan.FromDays(30);
    private const int AccessTokenExpiresInSeconds = 15 * 60;

    private readonly IApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICurrentTenant _currentTenant;

    public LoginCommandHandler(IApplicationDbContext db, IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator, ICurrentTenant currentTenant)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _currentTenant = currentTenant;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user is null || !user.Ativo || !_passwordHasher.Verify(request.Senha, user.SenhaHash))
        {
            throw new AuthenticationException();
        }

        if (!_currentTenant.IsResolved || _currentTenant.TenantId is null)
        {
            throw new InvalidOperationException("Tenant não resolvido — login exige host de empresa válido.");
        }

        user.RegistrarLogin();

        var accessToken = _jwtTokenGenerator.GerarAccessToken(user, _currentTenant.TenantId.Value);
        var refreshToken = string.Empty;

        if (request.ManterConectado)
        {
            refreshToken = _jwtTokenGenerator.GerarRefreshToken();
            var session = UserSession.Criar(user.Id, TokenHasher.Hash(refreshToken), RefreshTokenValidade);
            _db.UserSessions.Add(session);
        }

        await _db.SaveChangesAsync(cancellationToken);

        return new LoginResult(accessToken, refreshToken, AccessTokenExpiresInSeconds, user.Perfil.ToString());
    }
}
