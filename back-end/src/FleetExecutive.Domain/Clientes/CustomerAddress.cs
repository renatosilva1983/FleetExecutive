using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Clientes;

public enum TipoEndereco
{
    Cobranca = 1,
    Servico = 2,
}

/// <summary>Estrutura/06-modelo-de-dados.md — customer_addresses (endereço de cobrança pode divergir do de serviço).</summary>
public class CustomerAddress : Entity
{
    public Guid CustomerId { get; private set; }
    public TipoEndereco Tipo { get; private set; }
    public string Rua { get; private set; } = default!;
    public string? Numero { get; private set; }
    public string Cidade { get; private set; } = default!;
    public string Uf { get; private set; } = default!;
    public string? Cep { get; private set; }

    private CustomerAddress() { }

    public static CustomerAddress Criar(Guid customerId, TipoEndereco tipo, string rua, string cidade, string uf,
        string? numero = null, string? cep = null)
    {
        return new CustomerAddress
        {
            CustomerId = customerId,
            Tipo = tipo,
            Rua = rua.Trim(),
            Numero = numero?.Trim(),
            Cidade = cidade.Trim(),
            Uf = uf.Trim().ToUpperInvariant(),
            Cep = cep?.Trim(),
        };
    }
}
