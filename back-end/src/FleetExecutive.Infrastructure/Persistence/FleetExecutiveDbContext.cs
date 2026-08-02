using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Domain.Clientes;
using FleetExecutive.Domain.Financeiro;
using FleetExecutive.Domain.Orcamentos;
using FleetExecutive.Domain.Pedidos;
using FleetExecutive.Domain.Prestadores;
using FleetExecutive.Domain.Tarefas;
using FleetExecutive.Domain.Usuarios;
using FleetExecutive.Domain.Veiculos;
using FleetExecutive.Infrastructure.Persistence.Lookups;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Infrastructure.Persistence;

/// <summary>
/// Banco por-tenant (Estrutura/ESTRUTURA-PROJETO.md seção 4.3 — database-per-tenant). A connection
/// string usada é resolvida em runtime a partir do tenant corrente (ver DependencyInjection.cs),
/// nunca fixa em appsettings. DbSets de módulos de negócio são adicionados aqui conforme cada
/// módulo é implementado (Clientes, Pedidos, Veículos, ...).
/// </summary>
public class FleetExecutiveDbContext : MultiTenantDbContext, IApplicationDbContext
{
    public FleetExecutiveDbContext(IMultiTenantContextAccessor multiTenantContextAccessor, DbContextOptions<FleetExecutiveDbContext> options)
        : base(multiTenantContextAccessor, options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
    public DbSet<Garage> Garages => Set<Garage>();
    public DbSet<Fleet> Fleets => Set<Fleet>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<DriverLanguage> DriverLanguages => Set<DriverLanguage>();
    public DbSet<DriverDocument> DriverDocuments => Set<DriverDocument>();
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<QuoteService> QuoteServices => Set<QuoteService>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderAuditLog> OrderAuditLogs => Set<OrderAuditLog>();
    public DbSet<Commission> Commissions => Set<Commission>();
    public DbSet<Charge> Charges => Set<Charge>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FleetExecutiveDbContext).Assembly);

        // Tabelas-catálogo (enum -> tabela) e suas FKs, configuradas de forma centralizada a partir
        // do EnumLookupRegistry (em vez de repetir em cada Configuration). Cada catálogo tem a chave
        // "Id" = valor do enum (não gerada pelo banco) e "Nome" = identificador do enum.
        foreach (var t in EnumLookupRegistry.Tables)
        {
            var eb = modelBuilder.Entity(t.ClrType);
            eb.ToTable(t.TableName);
            eb.HasKey("Id");
            eb.Property("Id").ValueGeneratedNever();
            eb.Property("Nome").HasMaxLength(80).IsRequired();
        }

        // Cada propriedade enum de negócio vira uma FK (sem propriedade de navegação) para o "Id"
        // da tabela-catálogo correspondente. Como a propriedade e o "Id" são o MESMO enum, ambos
        // viram coluna int e o Postgres passa a garantir integridade referencial.
        foreach (var (entity, lookup, property) in EnumLookupRegistry.ForeignKeys)
        {
            modelBuilder.Entity(entity)
                .HasOne(lookup)
                .WithMany()
                .HasForeignKey(property);
        }
    }
}
