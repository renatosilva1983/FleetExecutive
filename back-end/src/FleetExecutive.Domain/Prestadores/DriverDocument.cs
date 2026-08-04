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
///
/// O Status de validade NÃO é calculado aqui: a regra vive na view vw_driver_documents (fonte única,
/// ver DatabaseViews) e é lida via DriverDocumentView. Assim relatórios (views materializadas) e o
/// app compartilham a MESMA definição, sem divergir.
/// </summary>
public class DriverDocument : Entity
{
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
}
