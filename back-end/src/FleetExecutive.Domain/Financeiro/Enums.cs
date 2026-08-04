using System.ComponentModel;

namespace FleetExecutive.Domain.Financeiro;

public enum TipoCobranca { Principal = 1, Extra = 2 }

/// <summary>Estrutura/03-sistema-atual-analise.md achado #7 — status reais observados em produção.</summary>
public enum StatusCobranca
{
    [Description("Recebida")]              Recebida = 1,
    [Description("Recebida pela Empresa")] RecebidaEmpresa = 2,
    [Description("A Vencer")]              AVencer = 3,
    [Description("Vencida")]               Vencida = 4,
}

public enum OrigemCobranca { Manual = 1, Asaas = 2 }

public enum StatusFatura
{
    [Description("A Faturar")] AFaturar = 1,
    Faturado = 2,
}

public enum TipoRecebedorComissao { Atendente = 1, Motorista = 2 }

public enum StatusComissao { Pendente = 1, Paga = 2 }

public enum TipoComissao { Normal = 1, Extra = 2 }
