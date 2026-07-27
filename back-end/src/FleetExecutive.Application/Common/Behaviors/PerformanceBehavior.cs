using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FleetExecutive.Application.Common.Behaviors;

/// <summary>
/// Mede o tempo de cada handler (Estrutura/12-modulos-financeiro-e-logging.md — "performance de
/// endpoints"). Toggle via configuração (hoje em appsettings "SystemSettings:PerformanceTrackingEnabled";
/// migra para a tabela system_settings quando o módulo de Configurações existir — ver
/// Estrutura/09-config-sistema-e-feature-flags.md). Quando desligado, é passthrough sem overhead
/// de Stopwatch.
/// </summary>
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly IConfiguration _configuration;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var enabled = _configuration.GetValue("SystemSettings:PerformanceTrackingEnabled", false);
        if (!enabled)
        {
            return await next();
        }

        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        _logger.LogInformation("Handler {RequestName} executado em {ElapsedMs}ms",
            typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);

        return response;
    }
}
