using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Usuarios;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FleetExecutive.Infrastructure.Identity;

/// <summary>
/// Claims mínimas do access token: sub, tenant_id, perfil (+ role padrão para compatibilidade com
/// [Authorize(Roles=...)]), driver_id/customer_id quando aplicável — ver
/// Estrutura/07-autenticacao-seguranca-rbac.md e Estrutura/08-perfis-e-permissoes.md.
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private const int AccessTokenExpiresInMinutes = 15;

    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarAccessToken(User user, Guid tenantId)
    {
        var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurada.");
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new("tenant_id", tenantId.ToString()),
            new("perfil", user.Perfil.ToString()),
            new(ClaimTypes.Role, user.Perfil.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        if (user.DriverId is not null)
        {
            claims.Add(new Claim("driver_id", user.DriverId.Value.ToString()));
        }

        if (user.CustomerId is not null)
        {
            claims.Add(new Claim("customer_id", user.CustomerId.Value.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(AccessTokenExpiresInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GerarRefreshToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
}
