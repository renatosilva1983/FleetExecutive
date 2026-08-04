using System.ComponentModel;

namespace FleetExecutive.Domain.Clientes;

public enum TipoPessoa
{
    [Description("Física")]   Fisica = 1,
    [Description("Jurídica")] Juridica = 2,
}
