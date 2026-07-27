namespace FleetExecutive.Application.Common.Interfaces;

/// <summary>
/// Identidade do usuário autenticado, resolvida exclusivamente das claims do JWT validado pelo
/// middleware — nunca de um parâmetro vindo do corpo da requisição. Ver
/// Estrutura/07-autenticacao-seguranca-rbac.md e Estrutura/08-perfis-e-permissoes.md.
/// </summary>
public interface ICurrentUser
{
    Guid? UserId { get; }
    string? Perfil { get; }
    /// <summary>Preenchido quando Perfil == Fornecedor (vínculo com Prestador/Motorista).</summary>
    Guid? DriverId { get; }
    /// <summary>Preenchido quando Perfil == Cliente (vínculo com Cliente).</summary>
    Guid? CustomerId { get; }
    bool IsAuthenticated { get; }
}
