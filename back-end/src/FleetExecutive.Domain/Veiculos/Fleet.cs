using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Veiculos;

/// <summary>
/// Categoria/modelo de veículo (ex. "Ônibus Executivo 46 Lugares") — distinto de Vehicle (unidade
/// física/placa). Ver Estrutura/03-sistema-atual-analise.md achado #1 e
/// Estrutura/06-modelo-de-dados.md. 1 Fleet possui N Vehicles.
/// </summary>
public class Fleet : Entity
{
    public string Titulo { get; private set; } = default!;
    public string? Descricao { get; private set; }
    public TipoVeiculo Tipo { get; private set; }
    public CategoriaVeiculo Categoria { get; private set; }
    public int Capacidade { get; private set; }
    public bool TemWc { get; private set; }
    public bool TemAr { get; private set; }
    public bool TemWifi { get; private set; }
    public bool TemAntt { get; private set; }
    public Guid? GaragemId { get; private set; }
    public bool Ativo { get; private set; } = true;

    private Fleet() { }

    public static Fleet Criar(string titulo, TipoVeiculo tipo, CategoriaVeiculo categoria, int capacidade,
        string? descricao = null, bool temWc = false, bool temAr = true, bool temWifi = false,
        bool temAntt = false, Guid? garagemId = null)
    {
        if (capacidade <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacidade), "Capacidade precisa ser maior que zero.");

        return new Fleet
        {
            Titulo = titulo.Trim(),
            Descricao = descricao?.Trim(),
            Tipo = tipo,
            Categoria = categoria,
            Capacidade = capacidade,
            TemWc = temWc,
            TemAr = temAr,
            TemWifi = temWifi,
            TemAntt = temAntt,
            GaragemId = garagemId,
        };
    }

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}
