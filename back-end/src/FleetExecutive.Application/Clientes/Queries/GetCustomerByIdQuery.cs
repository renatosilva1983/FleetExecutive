using FleetExecutive.Application.Clientes.Dtos;
using FleetExecutive.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Clientes.Queries;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto>;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly IApplicationDbContext _db;

    public GetCustomerByIdQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _db.Customers.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Cliente não encontrado.");

        return new CustomerDto(customer.Id, customer.Tipo.ToString(), customer.Nome, customer.CpfCnpj,
            customer.Email, customer.Telefone, customer.AtendenteId, customer.Ativo, customer.Observacoes,
            customer.Tags, customer.CreatedAt);
    }
}
