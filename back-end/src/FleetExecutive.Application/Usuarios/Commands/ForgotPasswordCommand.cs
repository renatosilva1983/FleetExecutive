using System.Security.Cryptography;
using FleetExecutive.Application.Common;
using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Usuarios;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Usuarios.Commands;

/// <summary>
/// Sempre retorna sucesso (Unit), exista ou não o e-mail — ver
/// Estrutura/07-autenticacao-seguranca-rbac.md, anti user-enumeration. O e-mail com o link só é
/// realmente enviado se o usuário existir; do ponto de vista do chamador da API não há diferença
/// observável.
/// </summary>
public record ForgotPasswordCommand(string Email) : IRequest;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
{
    private static readonly TimeSpan TokenValidade = TimeSpan.FromMinutes(45);

    private readonly IApplicationDbContext _db;
    private readonly IEmailSender _emailSender;

    public ForgotPasswordCommandHandler(IApplicationDbContext db, IEmailSender emailSender)
    {
        _db = db;
        _emailSender = emailSender;
    }

    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Ativo, cancellationToken);

        if (user is null)
        {
            return;
        }

        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var resetToken = PasswordResetToken.Criar(user.Id, TokenHasher.Hash(rawToken), TokenValidade);
        _db.PasswordResetTokens.Add(resetToken);
        await _db.SaveChangesAsync(cancellationToken);

        // O link real (com domínio do tenant) é montado na camada Api, que conhece o host da
        // requisição — aqui só disparamos o e-mail com o token bruto.
        await _emailSender.SendAsync(
            user.Email,
            "Redefinição de senha",
            $"Use o código a seguir para redefinir sua senha (válido por 45 minutos): {rawToken}",
            cancellationToken);
    }
}
