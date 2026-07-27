namespace FleetExecutive.Application.Common.Interfaces;

/// <summary>
/// Envio de e-mail (ex. "esqueci minha senha"). Implementação real usa as configurações de
/// Estrutura/09-config-sistema-e-feature-flags.md (system_settings, categoria Email) — ainda não
/// implementado; a Infrastructure atual registra um envio "no-op" que só loga, até o módulo de
/// Configurações do sistema existir.
/// </summary>
public interface IEmailSender
{
    Task SendAsync(string destinatario, string assunto, string corpo, CancellationToken cancellationToken = default);
}
