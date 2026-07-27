using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Orcamentos;
using FleetExecutive.Domain.Veiculos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Orcamentos.Commands;

public record CreateQuoteServiceItem(
    TipoServico TipoServico, SubtipoServico Subtipo, DateOnly DataIda, TimeOnly HoraIda, string Origem,
    string Destino, int NumPassageiros, DateOnly? DataVolta, TimeOnly? HoraVolta,
    TipoVeiculo? TipoVeiculoPreferido, IReadOnlyCollection<CaracteristicaServico>? Caracteristicas,
    string? IdiomaRequerido, string? Observacoes);

public record CreateQuoteCommand(
    Guid CustomerId, OrigemOrcamento Origem, decimal ValorEstimado, Guid? AtendenteId, DateOnly? DataServico,
    IReadOnlyCollection<CreateQuoteServiceItem>? Servicos) : IRequest<Guid>;

public class CreateQuoteCommandValidator : AbstractValidator<CreateQuoteCommand>
{
    public CreateQuoteCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.ValorEstimado).GreaterThanOrEqualTo(0);
    }
}

public class CreateQuoteCommandHandler : IRequestHandler<CreateQuoteCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateQuoteCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        var clienteExiste = await _db.Customers.AnyAsync(c => c.Id == request.CustomerId, cancellationToken);
        if (!clienteExiste)
        {
            throw new KeyNotFoundException("Cliente não encontrado.");
        }

        var quote = Quote.Criar(request.CustomerId, request.Origem, request.ValorEstimado, request.AtendenteId,
            request.DataServico);

        foreach (var s in request.Servicos ?? [])
        {
            var servico = QuoteService.Criar(quote.Id, s.TipoServico, s.Subtipo, s.DataIda, s.HoraIda, s.Origem,
                s.Destino, s.NumPassageiros, s.DataVolta, s.HoraVolta, s.TipoVeiculoPreferido, s.Observacoes);

            if (s.Caracteristicas is { Count: > 0 })
            {
                servico.DefinirCaracteristicas(s.Caracteristicas, s.IdiomaRequerido);
            }

            quote.AdicionarServico(servico);
        }

        _db.Quotes.Add(quote);
        await _db.SaveChangesAsync(cancellationToken);

        return quote.Id;
    }
}
