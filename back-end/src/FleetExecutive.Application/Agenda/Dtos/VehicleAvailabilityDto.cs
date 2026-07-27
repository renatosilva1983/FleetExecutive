namespace FleetExecutive.Application.Agenda.Dtos;

public record VehicleBookingDto(
    Guid OrderId, Guid OrderItemId, DateTimeOffset DataHoraIda, DateTimeOffset? DataHoraVolta,
    string Origem, string Destino, bool EmServicoAgora);

/// <summary>
/// Estrutura/10-modulos-comercial-operacional.md — Agenda de Veículos. Disponibilidade é derivada
/// de Vehicle (status administrativo) + OrderItem (reservas no período), nunca um campo próprio
/// armazenado — resolve a pendência registrada em docs/adr/0004-modulo-veiculos-frotas.md agora
/// que o módulo Pedidos existe.
/// </summary>
public record VehicleAvailabilityDto(
    Guid VehicleId, string NumeroOrdem, string Placa, string FleetTitulo, Guid? GaragemId,
    string StatusAdministrativo, IReadOnlyCollection<VehicleBookingDto> Reservas);
