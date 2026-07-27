using FleetExecutive.Application.Usuarios.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace FleetExecutive.Api.Controllers.Usuarios;

/// <summary>
/// Ver Estrutura/07-autenticacao-seguranca-rbac.md. Login e esqueci-senha usam a policy de rate
/// limiting "auth" (ver Program.cs) para mitigar brute force / user enumeration por volume.
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public record LoginRequest(string Email, string Senha, bool ManterConectado);

    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new LoginCommand(request.Email, request.Senha, request.ManterConectado), cancellationToken);
        return Ok(result);
    }

    public record ForgotPasswordRequest(string Email);

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ForgotPasswordCommand(request.Email), cancellationToken);
        // Resposta sempre genérica — nunca revelar se o e-mail existe (ver ForgotPasswordCommand).
        return Ok(new { message = "Se o e-mail existir, enviaremos instruções de redefinição de senha." });
    }

    public record ResetPasswordRequest(string Token, string NovaSenha);

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ResetPasswordCommand(request.Token, request.NovaSenha), cancellationToken);
        return Ok(new { message = "Senha redefinida com sucesso." });
    }

    public record ChangePasswordRequest(string SenhaAtual, string NovaSenha);

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ChangePasswordCommand(request.SenhaAtual, request.NovaSenha), cancellationToken);
        return Ok(new { message = "Senha alterada com sucesso." });
    }
}
