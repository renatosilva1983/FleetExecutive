using FleetExecutive.Domain.Clientes;
using FleetExecutive.Domain.Financeiro;
using FleetExecutive.Domain.Orcamentos;
using FleetExecutive.Domain.Pedidos;
using FleetExecutive.Domain.Prestadores;
using FleetExecutive.Domain.Tarefas;
using FleetExecutive.Domain.Usuarios;
using FleetExecutive.Domain.Veiculos;
using Microsoft.EntityFrameworkCore;

namespace FleetExecutive.Application.Common.Interfaces;

/// <summary>
/// Abstração do FleetExecutiveDbContext (per-tenant) para a Application layer não depender diretamente do
/// EF Core/Infrastructure (Clean Architecture — Estrutura/ESTRUTURA-PROJETO.md seção 4.1).
/// DbSets de cada módulo de negócio são adicionados aqui conforme implementados.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<PasswordResetToken> PasswordResetTokens { get; }
    DbSet<UserSession> UserSessions { get; }
    DbSet<Customer> Customers { get; }
    DbSet<CustomerAddress> CustomerAddresses { get; }
    DbSet<Garage> Garages { get; }
    DbSet<Fleet> Fleets { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<Driver> Drivers { get; }
    DbSet<DriverLanguage> DriverLanguages { get; }
    DbSet<DriverDocument> DriverDocuments { get; }
    DbSet<Quote> Quotes { get; }
    DbSet<QuoteService> QuoteServices { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<OrderAuditLog> OrderAuditLogs { get; }
    DbSet<Commission> Commissions { get; }
    DbSet<Charge> Charges { get; }
    DbSet<Invoice> Invoices { get; }
    DbSet<TaskItem> Tasks { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
