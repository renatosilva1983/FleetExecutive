using System.Net;
using System.Text.Json;
using FleetExecutive.Application.Common.Exceptions;
using ValidationException = FleetExecutive.Application.Common.Exceptions.ValidationException;

namespace FleetExecutive.Api.Middlewares;

/// <summary>
/// Traduz exceções da Application layer em respostas HTTP (Estrutura/ESTRUTURA-PROJETO.md seção
/// 4.2 — Middlewares/ExceptionHandlingMiddleware.cs). Nunca vaza stack trace/detalhes internos
/// para o cliente em produção.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, "Erro de validação", new { errors = ex.Errors });
        }
        catch (AuthenticationException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado processando {Path}", context.Request.Path);
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, string message,
        object? extra = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        object payload = extra is null ? new { message } : new { message, extra };
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
