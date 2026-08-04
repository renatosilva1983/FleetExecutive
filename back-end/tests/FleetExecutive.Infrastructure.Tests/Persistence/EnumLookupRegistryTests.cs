using FleetExecutive.Domain.Usuarios;
using FleetExecutive.Infrastructure.Persistence.Lookups;

namespace FleetExecutive.Infrastructure.Tests.Persistence;

/// <summary>
/// Garante a coerência do registro central de tabelas-catálogo (EnumLookupRegistry) — a fonte única
/// usada por OnModelCreating, pelo seeder e pela migration. Um erro aqui se propaga para o schema.
/// </summary>
public class EnumLookupRegistryTests
{
    [Fact]
    public void Tables_TemNomesDeTabelaUnicos()
    {
        var nomes = EnumLookupRegistry.Tables.Select(t => t.TableName).ToList();

        Assert.Equal(nomes.Count, nomes.Distinct().Count());
    }

    [Fact]
    public void Tables_TemTiposDeEnumUnicos()
    {
        var enums = EnumLookupRegistry.Tables.Select(t => t.EnumType).ToList();

        Assert.Equal(enums.Count, enums.Distinct().Count());
    }

    [Fact]
    public void ForeignKeys_ReferenciamLookupsRegistrados()
    {
        var lookupsRegistrados = EnumLookupRegistry.Tables.Select(t => t.ClrType).ToHashSet();

        foreach (var (_, lookup, _) in EnumLookupRegistry.ForeignKeys)
        {
            Assert.Contains(lookup, lookupsRegistrados);
        }
    }

    [Fact]
    public void ForeignKeys_PropriedadesExistemNasEntidades()
    {
        foreach (var (entity, _, property) in EnumLookupRegistry.ForeignKeys)
        {
            Assert.NotNull(entity.GetProperty(property));
        }
    }

    [Fact]
    public void Rows_DerivaParesIdNomeDosValoresDoEnum()
    {
        var perfil = EnumLookupRegistry.Tables.Single(t => t.EnumType == typeof(Perfil));

        var rows = perfil.Rows();

        Assert.Equal(Enum.GetValues<Perfil>().Length, rows.Count);
        Assert.Contains(((int)Perfil.Administrador, nameof(Perfil.Administrador)), rows);
        Assert.Contains(((int)Perfil.Cliente, nameof(Perfil.Cliente)), rows);
    }
}
