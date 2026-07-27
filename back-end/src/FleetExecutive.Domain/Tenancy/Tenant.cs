namespace FleetExecutive.Domain.Tenancy;

/// <summary>
/// Cadastro de empresa no banco "master" (Estrutura/06-modelo-de-dados.md — seção Master).
/// Cada tenant tem seu próprio banco de dados (database-per-tenant, ver
/// Estrutura/ESTRUTURA-PROJETO.md seção 4.3) — esta entidade só guarda os metadados de resolução
/// (identificador de host, connection string, branding), nunca dados de negócio.
/// </summary>
public class Tenant
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Identificador { get; private set; } = default!;
    public string Nome { get; private set; } = default!;
    public string Dominio { get; private set; } = default!;
    public string ConnectionString { get; private set; } = default!;
    public string? LogoUrl { get; private set; }
    public string? CorPrimaria { get; private set; }
    public bool Ativo { get; private set; } = true;
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    private Tenant() { }

    public Tenant(string identificador, string nome, string dominio, string connectionString)
    {
        Identificador = identificador;
        Nome = nome;
        Dominio = dominio;
        ConnectionString = connectionString;
    }

    public void AtualizarBranding(string? logoUrl, string? corPrimaria)
    {
        LogoUrl = logoUrl;
        CorPrimaria = corPrimaria;
    }

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}
