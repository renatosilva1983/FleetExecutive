using FleetExecutive.Domain.Prestadores;

namespace FleetExecutive.Application.Prestadores.ReadModels;

/// <summary>
/// Read model (somente leitura, sem chave) mapeado à view vw_driver_documents. O "Status" é
/// calculado NO BANCO — fonte única da regra de validade do documento (ver DatabaseViews). O app
/// apenas lê, sem reimplementar a regra em C#.
/// </summary>
public class DriverDocumentView
{
    public Guid Id { get; init; }
    public Guid DriverId { get; init; }
    public TipoDocumentoPrestador Tipo { get; init; }
    public string? Categoria { get; init; }
    public string? Numero { get; init; }
    public DateOnly? ValidoAte { get; init; }
    public string Status { get; init; } = string.Empty;
}
