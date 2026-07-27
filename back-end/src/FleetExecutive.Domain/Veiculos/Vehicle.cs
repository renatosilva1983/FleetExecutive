using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Veiculos;

/// <summary>
/// Unidade física (placa) vinculada a uma Fleet (categoria/modelo). O status aqui é o
/// administrativo (Ativo/Manutenção/Inativo) — a disponibilidade "em serviço agora"/"em rota" é
/// derivada dos pedidos/serviços ativos (módulo Pedidos, ainda não implementado), não um campo
/// estático deste registro. Ver Estrutura/06-modelo-de-dados.md.
/// </summary>
public class Vehicle : Entity
{
    public Guid FleetId { get; private set; }
    public string NumeroOrdem { get; private set; } = default!;
    public string Placa { get; private set; } = default!;
    public StatusVeiculo Status { get; private set; } = StatusVeiculo.Ativo;
    public Guid? GaragemId { get; private set; }
    public Guid? MotoristaHabitualId { get; private set; }
    public DateOnly? DisponivelDesde { get; private set; }
    public string? MotivoIndisponibilidade { get; private set; }
    public double? LocalizacaoLat { get; private set; }
    public double? LocalizacaoLng { get; private set; }

    private readonly List<VehicleFeature> _features = [];
    public IReadOnlyCollection<VehicleFeature> Features => _features.AsReadOnly();

    private Vehicle() { }

    public static Vehicle Criar(Guid fleetId, string numeroOrdem, string placa, Guid? garagemId = null,
        Guid? motoristaHabitualId = null)
    {
        return new Vehicle
        {
            FleetId = fleetId,
            NumeroOrdem = numeroOrdem.Trim(),
            Placa = placa.Trim().ToUpperInvariant(),
            GaragemId = garagemId,
            MotoristaHabitualId = motoristaHabitualId,
        };
    }

    public void DefinirFeatures(IEnumerable<VehicleFeature> features)
    {
        _features.Clear();
        _features.AddRange(features.Distinct());
    }

    public void MarcarEmManutencao(string motivo)
    {
        Status = StatusVeiculo.Manutencao;
        MotivoIndisponibilidade = motivo;
    }

    public void MarcarDisponivel(DateOnly? disponivelDesde = null)
    {
        Status = StatusVeiculo.Ativo;
        MotivoIndisponibilidade = null;
        DisponivelDesde = disponivelDesde;
    }

    public void Inativar() => Status = StatusVeiculo.Inativo;

    public void AtualizarLocalizacao(double lat, double lng)
    {
        LocalizacaoLat = lat;
        LocalizacaoLng = lng;
    }

    public void VincularGaragem(Guid garagemId) => GaragemId = garagemId;
    public void VincularMotoristaHabitual(Guid? motoristaId) => MotoristaHabitualId = motoristaId;
}
