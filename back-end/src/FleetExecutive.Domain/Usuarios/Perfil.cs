namespace FleetExecutive.Domain.Usuarios;

/// <summary>
/// Os 5 perfis definidos em Estrutura/08-perfis-e-permissoes.md. Não confundir com os níveis
/// reais do sistema legado (Administrador/Avançado/Vendedor Sênior) — ver Estrutura/02-dominio-e-glossario.md.
/// </summary>
public enum Perfil
{
    Administrador = 1,
    Operacional = 2,
    Financeiro = 3,
    Fornecedor = 4,
    Cliente = 5,
}
