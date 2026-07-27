using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Prestadores;

public enum StatusDocumento
{
    Valido,
    VencendoEmBreve,
    Vencido,
    SemValidade,
}

/// <summary>
/// CNH (com categoria A-E) ou ANTT (registro de empresa, sem categoria). Estrutura/11-modulos-cadastro.md
/// — alertar quando a validade estiver a menos de 60 dias de vencer.
/// </summary>
public class DriverDocument : Entity
{
    private const int DiasParaAlertaVencimento = 60;

    public Guid DriverId { get; private set; }
    public TipoDocumentoPrestador Tipo { get; private set; }
    public string? Categoria { get; private set; }
    public string? Numero { get; private set; }
    public DateOnly? ValidoAte { get; private set; }

    private DriverDocument() { }

    public static DriverDocument Criar(Guid driverId, TipoDocumentoPrestador tipo, string? categoria = null,
        string? numero = null, DateOnly? validoAte = null)
    {
        return new DriverDocument
        {
            DriverId = driverId,
            Tipo = tipo,
            Categoria = categoria?.Trim(),
            Numero = numero?.Trim(),
            ValidoAte = validoAte,
        };
    }

    public StatusDocumento Status
    {
        get
        {
            if (ValidoAte is null) return StatusDocumento.SemValidade;

            var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
            if (ValidoAte < hoje) return StatusDocumento.Vencido;
            if (ValidoAte <= hoje.AddDays(DiasParaAlertaVencimento)) return StatusDocumento.VencendoEmBreve;
            return StatusDocumento.Valido;
        }
    }
}
