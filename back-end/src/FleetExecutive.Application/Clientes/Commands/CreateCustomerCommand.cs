using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Clientes;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Clientes.Commands;

public record CreateCustomerCommand(
    TipoPessoa Tipo,
    string Nome,
    string Email,
    string? CpfCnpj,
    string? Telefone,
    Guid? AtendenteId,
    IReadOnlyCollection<string>? Tags) : IRequest<Guid>;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.CpfCnpj).MaximumLength(20);
    }
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateCustomerCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var emailNormalizado = request.Email.Trim().ToLowerInvariant();
        var duplicado = await _db.Customers.AnyAsync(c => c.Email == emailNormalizado, cancellationToken);
        if (duplicado)
        {
            throw new InvalidOperationException("Já existe um cliente cadastrado com este e-mail.");
        }

        var customer = Customer.Criar(request.Tipo, request.Nome, request.Email, request.CpfCnpj,
            request.Telefone, request.AtendenteId);

        if (request.Tags is { Count: > 0 })
        {
            customer.DefinirTags(request.Tags);
        }

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
