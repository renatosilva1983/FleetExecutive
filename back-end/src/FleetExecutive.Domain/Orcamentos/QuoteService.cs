using FleetExecutive.Domain.Common;
using FleetExecutive.Domain.Veiculos;

namespace FleetExecutive.Domain.Orcamentos;

/// <summary>Linha de serviço dentro de um orçamento (Estrutura/06-modelo-de-dados.md — quote_services).</summary>
public class QuoteService : Entity
{
    public Guid QuoteId { get; private set; }
    public TipoServico TipoServico { get; private set; }
    public SubtipoServico Subtipo { get; private set; }
    public DateOnly DataIda { get; private set; }
    public TimeOnly HoraIda { get; private set; }
    public string Origem { get; private set; } = default!;
    public string Destino { get; private set; } = default!;
    public DateOnly? DataVolta { get; private set; }
    public TimeOnly? HoraVolta { get; private set; }
    public int NumPassageiros { get; private set; }
    public TipoVeiculo? TipoVeiculoPreferido { get; private set; }
    public string? IdiomaRequerido { get; private set; }
    public string? Observacoes { get; private set; }

    private readonly List<CaracteristicaServico> _caracteristicas = [];
    public IReadOnlyCollection<CaracteristicaServico> Caracteristicas => _caracteristicas.AsReadOnly();

    private QuoteService() { }

    public static QuoteService Criar(Guid quoteId, TipoServico tipoServico, SubtipoServico subtipo,
        DateOnly dataIda, TimeOnly horaIda, string origem, string destino, int numPassageiros,
        DateOnly? dataVolta = null, TimeOnly? horaVolta = null, TipoVeiculo? tipoVeiculoPreferido = null,
        string? observacoes = null)
    {
        if (numPassageiros <= 0)
            throw new ArgumentOutOfRangeException(nameof(numPassageiros), "Número de passageiros precisa ser maior que zero.");
        if (subtipo == SubtipoServico.IdaVolta && dataVolta is null)
            throw new InvalidOperationException("Serviço 'Ida e Volta' exige data de volta.");

        return new QuoteService
        {
            QuoteId = quoteId,
            TipoServico = tipoServico,
            Subtipo = subtipo,
            DataIda = dataIda,
            HoraIda = horaIda,
            Origem = origem.Trim(),
            Destino = destino.Trim(),
            NumPassageiros = numPassageiros,
            DataVolta = dataVolta,
            HoraVolta = horaVolta,
            TipoVeiculoPreferido = tipoVeiculoPreferido,
            Observacoes = observacoes?.Trim(),
        };
    }

    public void DefinirCaracteristicas(IEnumerable<CaracteristicaServico> caracteristicas, string? idiomaRequerido = null)
    {
        _caracteristicas.Clear();
        _caracteristicas.AddRange(caracteristicas.Distinct());
        IdiomaRequerido = _caracteristicas.Contains(CaracteristicaServico.Bilingue) ? idiomaRequerido?.Trim() : null;
    }
}
