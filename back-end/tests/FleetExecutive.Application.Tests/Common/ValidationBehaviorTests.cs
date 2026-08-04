using FleetExecutive.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using ValidationException = FleetExecutive.Application.Common.Exceptions.ValidationException;

namespace FleetExecutive.Application.Tests.Common;

public class ValidationBehaviorTests
{
    // Request/validator de teste, isolados do domínio real.
    public record TestRequest(string Nome) : IRequest<string>;

    private sealed class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator() => RuleFor(x => x.Nome).NotEmpty();
    }

    private static RequestHandlerDelegate<string> Next(string retorno = "ok", Action? onCalled = null) =>
        () =>
        {
            onCalled?.Invoke();
            return Task.FromResult(retorno);
        };

    [Fact]
    public async Task SemValidators_ChamaProximoHandler()
    {
        var behavior = new ValidationBehavior<TestRequest, string>([]);

        var result = await behavior.Handle(new TestRequest("qualquer"), Next("proximo"), CancellationToken.None);

        Assert.Equal("proximo", result);
    }

    [Fact]
    public async Task RequestValido_ChamaProximoHandler()
    {
        var behavior = new ValidationBehavior<TestRequest, string>([new TestRequestValidator()]);

        var result = await behavior.Handle(new TestRequest("nome-valido"), Next("proximo"), CancellationToken.None);

        Assert.Equal("proximo", result);
    }

    [Fact]
    public async Task RequestInvalido_LancaValidationExceptionENaoChamaProximo()
    {
        var behavior = new ValidationBehavior<TestRequest, string>([new TestRequestValidator()]);
        var proximoChamado = false;

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            behavior.Handle(new TestRequest(""), Next(onCalled: () => proximoChamado = true), CancellationToken.None));

        Assert.False(proximoChamado);
        Assert.Contains(nameof(TestRequest.Nome), ex.Errors.Keys);
    }
}
