using System.Text;
using System.Threading.RateLimiting;
using FleetExecutive.Api.Middlewares;
using FleetExecutive.Application;
using FleetExecutive.Infrastructure;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Swagger / OpenAPI (Swashbuckle) com suporte ao esquema JWT Bearer, para autorizar as
// requisições diretamente pela UI (botão "Authorize").
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FleetExecutive.Api",
        Version = "v1",
    });

    // Evita colisão de schemaId entre tipos de mesmo nome curto em controllers diferentes
    // (ex.: TasksController+UpdateStatusRequest x QuotesController+UpdateStatusRequest).
    // Usa o nome totalmente qualificado, com tipos aninhados separados por ponto.
    options.CustomSchemaIds(type => type.FullName!.Replace("+", "."));

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT. Exemplo: \"eyJhbGciOi...\"",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            Array.Empty<string>()
        },
    });
});

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
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FleetExecutive.Api v1");
    });
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

// Torna a classe Program acessível para o WebApplicationFactory<Program> usado nos testes de
// integração (FleetExecutive.Api.IntegrationTests). Com top-level statements, o Program gerado é
// internal — este partial público apenas expõe o ponto de entrada para a hospedagem em memória.
public partial class Program;
