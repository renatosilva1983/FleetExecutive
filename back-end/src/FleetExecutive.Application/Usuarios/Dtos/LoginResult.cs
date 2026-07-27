namespace FleetExecutive.Application.Usuarios.Dtos;

public record LoginResult(string AccessToken, string RefreshToken, int ExpiresInSeconds, string Perfil);
