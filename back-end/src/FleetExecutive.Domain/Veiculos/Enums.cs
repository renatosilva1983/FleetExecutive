namespace FleetExecutive.Domain.Veiculos;

/// <summary>Estrutura/03-sistema-atual-analise.md — tipos reais observados em produção (mais amplos que o protótipo).</summary>
public enum TipoVeiculo
{
    Carro = 1,
    Van = 2,
    MicroOnibus = 3,
    Onibus = 4,
}

public enum CategoriaVeiculo
{
    Executivo = 1,
    Rodoviario = 2,
    SemiLeito = 3,
    Leito = 4,
    DoubleDeck = 5,
}

public enum StatusVeiculo
{
    Ativo = 1,
    Manutencao = 2,
    Inativo = 3,
}

/// <summary>Estrutura/04-prototipo-mapeamento-telas.md — características combináveis do veículo.</summary>
public enum VehicleFeature
{
    Wc = 1,
    Ar = 2,
    Bordo = 3,
    Usb = 4,
    Adesivacao = 5,
    Tv = 6,
    WiFi = 7,
    Blindado = 8,
}
