namespace FleetExecutive.Application.Veiculos.Dtos;

public record FleetDto(
    Guid Id, string Titulo, string? Descricao, string Tipo, string Categoria, int Capacidade,
    bool TemWc, bool TemAr, bool TemWifi, bool TemAntt, Guid? GaragemId, bool Ativo);
