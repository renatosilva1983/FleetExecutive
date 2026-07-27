using System.Text;
using System.Threading.RateLimiting;
using FleetExecutive.Api.Middlewares;
using FleetExecutive.Application;
using FleetExecutive.Infrastructure;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Autenticação JWT (ver Estrutura/07-autenticacao-seguranca-rbac.md). Claims mínimas: sub,
// tenant_id, perfil. Chave/emissor/audiência configuráveis por ambiente (nunca hardcoded).
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key não configurada.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(1),
        };
    });

builder.Services.AddAuthorization();

// Rate limiting nos endpoints de autenticação (Estrutura/07-autenticacao-seguranca-rbac.md —
// mitigar brute force / user enumeration por volume). Partição por IP: 5 tentativas / 15 min.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("auth", context => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "desconhecido",
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(15),
            QueueLimit = 0,
        }));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Primeiro middleware do pipeline — precisa capturar exceções de qualquer etapa seguinte.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// Resolução de tenant por Host precisa vir antes de autenticação/autorização, para que o
// TenantClaimValidationMiddleware abaixo consiga comparar o tenant resolvido com a claim do JWT.
app.UseMultiTenant();

app.UseRateLimiter();

app.UseAuthentication();
app.UseMiddleware<TenantClaimValidationMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
