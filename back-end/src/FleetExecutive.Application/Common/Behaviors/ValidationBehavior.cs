using FluentValidation;
using MediatR;
using ValidationException = FleetExecutive.Application.Common.Exceptions.ValidationException;

namespace FleetExecutive.Application.Common.Behaviors;

/// <summary>
/// Pipeline behavior do MediatR citado em Estrutura/ESTRUTURA-PROJETO.md seção 4.2
/// (Application/Common/Behaviors) — roda os validators FluentValidation de cada Command/Query
/// antes do handler. Lança ValidationException (400) se algum validator falhar.
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToList();

        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
