using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Clientes;

/// <summary>
/// Estrutura/06-modelo-de-dados.md — módulo Clientes (piloto do roadmap v1, ver
/// Estrutura/13-deploy-multitenant-e-roadmap.md). Volume real observado em produção é grande
/// (~5.900 clientes, ver Estrutura/03-sistema-atual-analise.md achado #9) — toda consulta de
/// listagem precisa paginar e filtrar por índice (nome/e-mail/CNPJ/status), nunca trazer tudo.
/// </summary>
public class Customer : Entity
{
    public TipoPessoa Tipo { get; private set; }
    public string Nome { get; private set; } = default!;
    public string? CpfCnpj { get; private set; }
    public string Email { get; private set; } = default!;
    public string? Telefone { get; private set; }
    public Guid? AtendenteId { get; private set; }
    public bool Ativo { get; private set; } = true;
    public string? Observacoes { get; private set; }

    private readonly List<string> _tags = [];
    public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

    private readonly List<CustomerAddress> _enderecos = [];
    public IReadOnlyCollection<CustomerAddress> Enderecos => _enderecos.AsReadOnly();

    private Customer() { }

    public static Customer Criar(TipoPessoa tipo, string nome, string email, string? cpfCnpj = null,
        string? telefone = null, Guid? atendenteId = null)
    {
        return new Customer
        {
            Tipo = tipo,
            Nome = nome.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            CpfCnpj = cpfCnpj?.Trim(),
            Telefone = telefone?.Trim(),
            AtendenteId = atendenteId,
        };
    }

    public void Atualizar(string nome, string email, string? cpfCnpj, string? telefone, Guid? atendenteId,
        string? observacoes)
    {
        Nome = nome.Trim();
        Email = email.Trim().ToLowerInvariant();
        CpfCnpj = cpfCnpj?.Trim();
        Telefone = telefone?.Trim();
        AtendenteId = atendenteId;
        Observacoes = observacoes;
        MarkUpdated(AtendenteId);
    }

    public void DefinirTags(IEnumerable<string> tags)
    {
        _tags.Clear();
        _tags.AddRange(tags.Select(t => t.Trim()).Where(t => t.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase));
    }

    public void AdicionarEndereco(CustomerAddress endereco) => _enderecos.Add(endereco);

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}
