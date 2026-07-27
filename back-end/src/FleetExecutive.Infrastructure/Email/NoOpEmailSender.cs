using FleetExecutive.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FleetExecutive.Infrastructure.Email;

/// <summary>
/// Implementação provisória — só loga (nunca a senha/token em texto claro fora deste log
/// controlado de dev). Substituir por SMTP real configurado via system_settings quando o módulo
/// de Configurações existir (Estrutura/09-config-sistema-e-feature-flags.md, categoria Email).
/// </summary>
public class NoOpEmailSender : IEmailSender
{
    private readonly ILogger<NoOpEmailSender> _logger;

    public NoOpEmailSender(ILogger<NoOpEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string destinatario, string assunto, string corpo, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning(
            "IEmailSender ainda não configurado (NoOpEmailSender) — e-mail para {Destinatario} não foi enviado de verdade. Assunto: {Assunto}",
            destinatario, assunto);
        return Task.CompletedTask;
    }
}
