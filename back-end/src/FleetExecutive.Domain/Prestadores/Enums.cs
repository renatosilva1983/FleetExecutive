namespace FleetExecutive.Domain.Prestadores;

public enum TipoPrestador
{
    MotoristaAutonomo = 1,
    EmpresaFretamento = 2,
}

public enum NivelIdioma
{
    Basico = 1,
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
    Cnh = 1,
    Antt = 2,
}
