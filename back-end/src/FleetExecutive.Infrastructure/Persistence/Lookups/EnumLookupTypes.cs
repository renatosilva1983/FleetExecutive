using FleetExecutive.Domain.Clientes;
using FleetExecutive.Domain.Financeiro;
using FleetExecutive.Domain.Orcamentos;
using FleetExecutive.Domain.Pedidos;
using FleetExecutive.Domain.Prestadores;
using FleetExecutive.Domain.Tarefas;
using FleetExecutive.Domain.Usuarios;
using FleetExecutive.Domain.Veiculos;

namespace FleetExecutive.Infrastructure.Persistence.Lookups;

// Um tipo CLR concreto por tabela-catálogo (o EF precisa de um tipo por tabela). São classes vazias
// de propósito: toda a estrutura vem de EnumLookup<TEnum>. O mapeamento (tabela/chave) e a semeadura
// são feitos de forma centralizada em EnumLookupRegistry / FleetExecutiveDbContext / EnumLookupSeeder.

// Financeiro
public sealed class TipoCobrancaLookup : EnumLookup<TipoCobranca> { }
public sealed class StatusCobrancaLookup : EnumLookup<StatusCobranca> { }
public sealed class OrigemCobrancaLookup : EnumLookup<OrigemCobranca> { }
public sealed class TipoComissaoLookup : EnumLookup<TipoComissao> { }
public sealed class TipoRecebedorComissaoLookup : EnumLookup<TipoRecebedorComissao> { }
public sealed class StatusComissaoLookup : EnumLookup<StatusComissao> { }
public sealed class StatusFaturaLookup : EnumLookup<StatusFatura> { }

// Clientes
public sealed class TipoPessoaLookup : EnumLookup<TipoPessoa> { }
public sealed class TipoEnderecoLookup : EnumLookup<TipoEndereco> { }

// Veículos
public sealed class TipoVeiculoLookup : EnumLookup<TipoVeiculo> { }
public sealed class CategoriaVeiculoLookup : EnumLookup<CategoriaVeiculo> { }
public sealed class StatusVeiculoLookup : EnumLookup<StatusVeiculo> { }

// Prestadores
public sealed class TipoPrestadorLookup : EnumLookup<TipoPrestador> { }
public sealed class NivelIdiomaLookup : EnumLookup<NivelIdioma> { }
public sealed class TipoDocumentoPrestadorLookup : EnumLookup<TipoDocumentoPrestador> { }

// Orçamentos
public sealed class OrigemOrcamentoLookup : EnumLookup<OrigemOrcamento> { }
public sealed class StatusFunilLookup : EnumLookup<StatusFunil> { }
public sealed class TipoServicoLookup : EnumLookup<TipoServico> { }
public sealed class SubtipoServicoLookup : EnumLookup<SubtipoServico> { }

// Pedidos
public sealed class StatusComercialLookup : EnumLookup<StatusComercial> { }
public sealed class StatusOperacionalLookup : EnumLookup<StatusOperacional> { }

// Tarefas
public sealed class PrioridadeTarefaLookup : EnumLookup<PrioridadeTarefa> { }
public sealed class StatusTarefaLookup : EnumLookup<StatusTarefa> { }
public sealed class VinculoTarefaLookup : EnumLookup<VinculoTarefa> { }

// Usuários
public sealed class PerfilLookup : EnumLookup<Perfil> { }
