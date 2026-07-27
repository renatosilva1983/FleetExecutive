namespace FleetExecutive.Application.Veiculos.Dtos;

public record VehicleDto(
    Guid Id, Guid FleetId, string NumeroOrdem, string Placa, string Status, Guid? GaragemId,
    Guid? MotoristaHabitualId, DateOnly? DisponivelDesde, string? MotivoIndisponibilidade,
    IReadOnlyCollection<string> Features);

public record VehicleListItemDto(
    Guid Id, string NumeroOrdem, string Placa, string FleetTitulo, string Tipo, int Capacidade,
    string Status, Guid? GaragemId);
