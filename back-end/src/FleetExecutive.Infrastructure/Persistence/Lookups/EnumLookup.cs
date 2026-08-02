namespace FleetExecutive.Infrastructure.Persistence.Lookups;

/// <summary>
/// Base de toda tabela-catálogo derivada de um enum. "Id" é o PRÓPRIO valor do enum (gravado como
/// int); "Nome" é o identificador legível (o nome do enum em C#). As entidades de negócio passam a
/// ter uma FK para o "Id" desta tabela — o banco garante que só valores válidos sejam gravados.
/// </summary>
public abstract class EnumLookup<TEnum> where TEnum : struct, Enum
{
    public TEnum Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}

/// <summary>Descreve uma tabela-catálogo: tipo CLR do lookup, tipo do enum e nome da tabela.</summary>
public sealed class EnumLookupDescriptor
{
    public required Type ClrType { get; init; }
    public required Type EnumType { get; init; }
    public required string TableName { get; init; }

    /// <summary>Os pares (Id, Nome) esperados, derivados dos valores do enum.</summary>
    public IReadOnlyList<(int Id, string Nome)> Rows() =>
        Enum.GetValues(EnumType).Cast<object>()
            .Select(v => (Convert.ToInt32(v), v.ToString()!))
            .ToList();

    public static EnumLookupDescriptor Create<TLookup, TEnum>(string tableName)
        where TLookup : EnumLookup<TEnum>
        where TEnum : struct, Enum
        => new() { ClrType = typeof(TLookup), EnumType = typeof(TEnum), TableName = tableName };
}
