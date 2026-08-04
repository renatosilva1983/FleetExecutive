using System.ComponentModel;

namespace FleetExecutive.Domain.Prestadores;

public enum TipoPrestador
{
    [Description("Motorista Autônomo")]    MotoristaAutonomo = 1,
    [Description("Empresa de Fretamento")] EmpresaFretamento = 2,
}

public enum NivelIdioma
{
    [Description("Básico")] Basico = 1,
    Fluente = 2,
}

/// <summary>Estrutura/04-prototipo-mapeamento-telas.md seção 4 — capacidades combináveis do prestador.</summary>
public enum DriverCapability
{
    ServicoBordo = 1,
    AceitaAdesivacao = 2,
    Blindado = 3,
}

/// <summary>CNH cobre categorias A-E; ANTT é um registro de empresa, sem categoria.</summary>
public enum TipoDocumentoPrestador
{
    [Description("CNH")]  Cnh = 1,
    [Description("ANTT")] Antt = 2,
}
