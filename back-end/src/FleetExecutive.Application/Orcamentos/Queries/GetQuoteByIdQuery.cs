using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Orcamentos.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Orcamentos.Queries;

public record GetQuoteByIdQuery(Guid Id) : IRequest<QuoteDto>;

public class GetQuoteByIdQueryHandler : IRequestHandler<GetQuoteByIdQuery, QuoteDto>
{
    private readonly IApplicationDbContext _db;

    public GetQuoteByIdQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<QuoteDto> Handle(GetQuoteByIdQuery request, CancellationToken cancellationToken)
    {
        var quote = await _db.Quotes.AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        var servicos = await _db.QuoteServices.AsNoTracking()
            .Where(s => s.QuoteId == request.Id)
            .ToListAsync(cancellationToken);

        var servicosDto = servicos.Select(s => new QuoteServiceDto(s.Id, s.TipoServico.ToString(),
            s.Subtipo.ToString(), s.DataIda, s.HoraIda, s.Origem, s.Destino, s.DataVolta, s.HoraVolta,
            s.NumPassageiros, s.TipoVeiculoPreferido?.ToString(),
            s.Caracteristicas.Select(c => c.ToString()).ToList(), s.IdiomaRequerido, s.Observacoes)).ToList();

        return new QuoteDto(quote.Id, quote.CustomerId, quote.AtendenteId, quote.Origem.ToString(),
            quote.Status.ToString(), quote.DataServico, quote.ValorEstimado, quote.MotivoPerda, quote.CreatedAt,
            servicosDto);
    }
}
