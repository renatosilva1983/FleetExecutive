using FleetExecutive.Domain.Clientes;
using FleetExecutive.Domain.Financeiro;
using FleetExecutive.Domain.Orcamentos;
using FleetExecutive.Domain.Pedidos;
using FleetExecutive.Domain.Prestadores;
using FleetExecutive.Domain.Tarefas;
using FleetExecutive.Domain.Usuarios;
using FleetExecutive.Domain.Veiculos;

namespace FleetExecutive.Infrastructure.Persistence.Lookups;

/// <summary>
/// Fonte única da verdade das tabelas-catálogo: quais existem (Tables) e quais colunas de entidade
/// apontam para elas (ForeignKeys). Usado em três lugares:
///  - FleetExecutiveDbContext.OnModelCreating: mapeia as tabelas e cria as FKs;
///  - EnumLookupSeeder: semeia e valida as linhas na publicação;
///  - a migration gerada reflete tudo isso no schema.
/// </summary>
public static class EnumLookupRegistry
{
    public static readonly IReadOnlyList<EnumLookupDescriptor> Tables = new[]
    {
        EnumLookupDescriptor.Create<TipoCobrancaLookup, TipoCobranca>("tipo_cobranca"),
        EnumLookupDescriptor.Create<StatusCobrancaLookup, StatusCobranca>("status_cobranca"),
        EnumLookupDescriptor.Create<OrigemCobrancaLookup, OrigemCobranca>("origem_cobranca"),
        EnumLookupDescriptor.Create<TipoComissaoLookup, TipoComissao>("tipo_comissao"),
        EnumLookupDescriptor.Create<TipoRecebedorComissaoLookup, TipoRecebedorComissao>("tipo_recebedor_comissao"),
        EnumLookupDescriptor.Create<StatusComissaoLookup, StatusComissao>("status_comissao"),
        EnumLookupDescriptor.Create<StatusFaturaLookup, StatusFatura>("status_fatura"),
        EnumLookupDescriptor.Create<TipoPessoaLookup, TipoPessoa>("tipo_pessoa"),
        EnumLookupDescriptor.Create<TipoEnderecoLookup, TipoEndereco>("tipo_endereco"),
        EnumLookupDescriptor.Create<TipoVeiculoLookup, TipoVeiculo>("tipo_veiculo"),
        EnumLookupDescriptor.Create<CategoriaVeiculoLookup, CategoriaVeiculo>("categoria_veiculo"),
        EnumLookupDescriptor.Create<StatusVeiculoLookup, StatusVeiculo>("status_veiculo"),
        EnumLookupDescriptor.Create<TipoPrestadorLookup, TipoPrestador>("tipo_prestador"),
        EnumLookupDescriptor.Create<NivelIdiomaLookup, NivelIdioma>("nivel_idioma"),
        EnumLookupDescriptor.Create<TipoDocumentoPrestadorLookup, TipoDocumentoPrestador>("tipo_documento_prestador"),
        EnumLookupDescriptor.Create<OrigemOrcamentoLookup, OrigemOrcamento>("origem_orcamento"),
        EnumLookupDescriptor.Create<StatusFunilLookup, StatusFunil>("status_funil"),
        EnumLookupDescriptor.Create<TipoServicoLookup, TipoServico>("tipo_servico"),
        EnumLookupDescriptor.Create<SubtipoServicoLookup, SubtipoServico>("subtipo_servico"),
        EnumLookupDescriptor.Create<StatusComercialLookup, StatusComercial>("status_comercial"),
        EnumLookupDescriptor.Create<StatusOperacionalLookup, StatusOperacional>("status_operacional"),
        EnumLookupDescriptor.Create<PrioridadeTarefaLookup, PrioridadeTarefa>("prioridade_tarefa"),
        EnumLookupDescriptor.Create<StatusTarefaLookup, StatusTarefa>("status_tarefa"),
        EnumLookupDescriptor.Create<VinculoTarefaLookup, VinculoTarefa>("vinculo_tarefa"),
        EnumLookupDescriptor.Create<PerfilLookup, Perfil>("perfil"),
    };

    /// <summary>(entidade, tabela-catálogo, propriedade enum que vira a FK).</summary>
    public static readonly IReadOnlyList<(Type Entity, Type Lookup, string Property)> ForeignKeys = new[]
    {
        (typeof(Charge), typeof(TipoCobrancaLookup), nameof(Charge.Tipo)),
        (typeof(Charge), typeof(StatusCobrancaLookup), nameof(Charge.Status)),
        (typeof(Charge), typeof(OrigemCobrancaLookup), nameof(Charge.Origem)),
        (typeof(Commission), typeof(TipoComissaoLookup), nameof(Commission.Tipo)),
        (typeof(Commission), typeof(TipoRecebedorComissaoLookup), nameof(Commission.RecebedorTipo)),
        (typeof(Commission), typeof(StatusComissaoLookup), nameof(Commission.Status)),
        (typeof(Invoice), typeof(StatusFaturaLookup), nameof(Invoice.Status)),
        (typeof(Customer), typeof(TipoPessoaLookup), nameof(Customer.Tipo)),
        (typeof(CustomerAddress), typeof(TipoEnderecoLookup), nameof(CustomerAddress.Tipo)),
        (typeof(Fleet), typeof(TipoVeiculoLookup), nameof(Fleet.Tipo)),
        (typeof(Fleet), typeof(CategoriaVeiculoLookup), nameof(Fleet.Categoria)),
        (typeof(Vehicle), typeof(StatusVeiculoLookup), nameof(Vehicle.Status)),
        (typeof(Driver), typeof(TipoPrestadorLookup), nameof(Driver.Tipo)),
        (typeof(DriverLanguage), typeof(NivelIdiomaLookup), nameof(DriverLanguage.Nivel)),
        (typeof(DriverDocument), typeof(TipoDocumentoPrestadorLookup), nameof(DriverDocument.Tipo)),
        (typeof(Quote), typeof(OrigemOrcamentoLookup), nameof(Quote.Origem)),
        (typeof(Quote), typeof(StatusFunilLookup), nameof(Quote.Status)),
        (typeof(QuoteService), typeof(TipoServicoLookup), nameof(QuoteService.TipoServico)),
        (typeof(QuoteService), typeof(SubtipoServicoLookup), nameof(QuoteService.Subtipo)),
        (typeof(QuoteService), typeof(TipoVeiculoLookup), nameof(QuoteService.TipoVeiculoPreferido)),
        (typeof(Order), typeof(OrigemOrcamentoLookup), nameof(Order.Origem)),
        (typeof(Order), typeof(StatusComercialLookup), nameof(Order.StatusComercial)),
        (typeof(Order), typeof(StatusOperacionalLookup), nameof(Order.StatusOperacional)),
        (typeof(OrderItem), typeof(SubtipoServicoLookup), nameof(OrderItem.Tipo)),
        (typeof(TaskItem), typeof(PrioridadeTarefaLookup), nameof(TaskItem.Prioridade)),
        (typeof(TaskItem), typeof(StatusTarefaLookup), nameof(TaskItem.Status)),
        (typeof(TaskItem), typeof(VinculoTarefaLookup), nameof(TaskItem.VinculoTipo)),
        (typeof(User), typeof(PerfilLookup), nameof(User.Perfil)),
    };
}
