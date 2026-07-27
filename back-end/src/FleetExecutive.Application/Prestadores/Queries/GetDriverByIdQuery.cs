using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Prestadores.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Prestadores.Queries;

public record GetDriverByIdQuery(Guid Id) : IRequest<DriverDto>;

public class GetDriverByIdQueryHandler : IRequestHandler<GetDriverByIdQuery, DriverDto>
{
    private readonly IApplicationDbContext _db;

    public GetDriverByIdQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<DriverDto> Handle(GetDriverByIdQuery request, CancellationToken cancellationToken)
    {
        var driver = await _db.Drivers.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Prestador não encontrado.");

        var idiomas = await _db.DriverLanguages.AsNoTracking()
            .Where(l => l.DriverId == request.Id)
            .Select(l => new DriverLanguageDto(l.Id, l.Idioma, l.Nivel.ToString()))
            .ToListAsync(cancellationToken);

        var documentos = await _db.DriverDocuments.AsNoTracking()
            .Where(doc => doc.DriverId == request.Id)
            .ToListAsync(cancellationToken);

        return new DriverDto(driver.Id, driver.Nome, driver.Tipo.ToString(), driver.Ativo, driver.Telefone,
            driver.Referencia, driver.ComissaoPercentual, driver.Indicacao, driver.FonteIndicacao,
            driver.Capacidades.Select(c => c.ToString()).ToList(),
            idiomas,
            documentos.Select(doc => new DriverDocumentDto(doc.Id, doc.Tipo.ToString(), doc.Categoria, doc.Numero,
                doc.ValidoAte, doc.Status.ToString())).ToList());
    }
}
