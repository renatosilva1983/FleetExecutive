using FleetExecutive.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Clientes.Commands;

public record UpdateCustomerCommand(
    Guid Id,
    string Nome,
    string Email,
    string? CpfCnpj,
    string? Telefone,
    Guid? AtendenteId,
    string? Observacoes,
    IReadOnlyCollection<string>? Tags) : IRequest;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateCustomerCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Cliente não encontrado.");

        customer.Atualizar(request.Nome, request.Email, request.CpfCnpj, request.Telefone,
            request.AtendenteId, request.Observacoes);

        if (request.Tags is not null)
        {
            customer.DefinirTags(request.Tags);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
