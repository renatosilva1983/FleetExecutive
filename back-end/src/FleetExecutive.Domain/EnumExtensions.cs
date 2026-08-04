using System.ComponentModel;
using System.Reflection;

namespace FleetExecutive.Domain;

/// <summary>Utilitários para os enums do domínio.</summary>
public static class EnumExtensions
{
    /// <summary>
    /// Rótulo de exibição do valor: usa o texto do atributo [Description] quando presente; caso
    /// contrário, cai no nome do enum em C#. Nunca lança — valores sem atributo (ou até fora do
    /// enum) voltam como o próprio texto do valor. Fonte única desta conversão (usada no seed das
    /// tabelas-catálogo e disponível para API/telas).
    /// </summary>
    public static string GetDescription(this Enum value)
    {
        var nome = value.ToString();
        var descricao = value.GetType().GetField(nome)?
            .GetCustomAttribute<DescriptionAttribute>()?.Description;
        return string.IsNullOrWhiteSpace(descricao) ? nome : descricao;
    }
}
