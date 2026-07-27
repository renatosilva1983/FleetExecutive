using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Veiculos;

/// <summary>
/// Estrutura/03-sistema-atual-analise.md — empresa opera garagens em dezenas de cidades, não só
/// "Principal/Secundária" como o protótipo sugeria. GarageGroup/Seguradoras/Bloqueios/Opcionais
/// ficam para uma iteração seguinte (ver ADR do módulo) — Garage é o cadastro mínimo necessário
/// para Fleet/Vehicle funcionarem.
/// </summary>
public class Garage : Entity
{
    public string Nome { get; private set; } = default!;
    public string Cidade { get; private set; } = default!;
    public string Uf { get; private set; } = default!;
    public bool Ativo { get; private set; } = true;

    private Garage() { }

    public static Garage Criar(string nome, string cidade, string uf)
    {
        return new Garage { Nome = nome.Trim(), Cidade = cidade.Trim(), Uf = uf.Trim().ToUpperInvariant() };
    }

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}
