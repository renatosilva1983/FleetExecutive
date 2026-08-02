using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetExecutive.Infrastructure.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class InitialTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categoria_veiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoria_veiculo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "garages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Cidade = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Uf = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_garages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "nivel_idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nivel_idioma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "order_audit_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    AutorId = table.Column<Guid>(type: "uuid", nullable: true),
                    TipoEvento = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    CampoAlterado = table.Column<string>(type: "text", nullable: true),
                    ValorAnterior = table.Column<string>(type: "text", nullable: true),
                    ValorNovo = table.Column<string>(type: "text", nullable: true),
                    Justificativa = table.Column<string>(type: "text", nullable: true),
                    CriadoEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_audit_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "origem_cobranca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_origem_cobranca", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "origem_orcamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_origem_orcamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "password_reset_tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiraEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UsadoEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_reset_tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "perfil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perfil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "prioridade_tarefa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prioridade_tarefa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status_cobranca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status_cobranca", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status_comercial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status_comercial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status_comissao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status_comissao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status_fatura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status_fatura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status_funil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status_funil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status_operacional",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status_operacional", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status_tarefa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status_tarefa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status_veiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status_veiculo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subtipo_servico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subtipo_servico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_cobranca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_cobranca", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_comissao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_comissao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_documento_prestador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_documento_prestador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_endereco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_endereco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_pessoa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_pessoa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_prestador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_prestador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_recebedor_comissao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_recebedor_comissao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_servico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_servico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_veiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_veiculo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshTokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiraEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RevogadoEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vinculo_tarefa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vinculo_tarefa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    SenhaHash = table.Column<string>(type: "text", nullable: false),
                    Perfil = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    UltimoLoginEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_perfil_Perfil",
                        column: x => x.Perfil,
                        principalTable: "perfil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    GeradoEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_invoices_status_fatura_Status",
                        column: x => x.Status,
                        principalTable: "status_fatura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AtendenteId = table.Column<Guid>(type: "uuid", nullable: true),
                    Origem = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataServico = table.Column<DateOnly>(type: "date", nullable: true),
                    ValorEstimado = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    MotivoPerda = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quotes_origem_orcamento_Origem",
                        column: x => x.Origem,
                        principalTable: "origem_orcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_quotes_status_funil_Status",
                        column: x => x.Status,
                        principalTable: "status_funil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuoteId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AtendenteId = table.Column<Guid>(type: "uuid", nullable: true),
                    Origem = table.Column<int>(type: "integer", nullable: false),
                    StatusComercial = table.Column<int>(type: "integer", nullable: false),
                    StatusOperacional = table.Column<int>(type: "integer", nullable: false),
                    FormaPagamento = table.Column<string>(type: "text", nullable: true),
                    CodigoAceiteTermos = table.Column<string>(type: "text", nullable: true),
                    AceiteEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    MotivoCancelamento = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orders_origem_orcamento_Origem",
                        column: x => x.Origem,
                        principalTable: "origem_orcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_status_comercial_StatusComercial",
                        column: x => x.StatusComercial,
                        principalTable: "status_comercial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_status_operacional_StatusOperacional",
                        column: x => x.StatusOperacional,
                        principalTable: "status_operacional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "charges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Origem = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    VenceEm = table.Column<DateOnly>(type: "date", nullable: false),
                    PagoEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ImpostoRetido = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_charges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_charges_origem_cobranca_Origem",
                        column: x => x.Origem,
                        principalTable: "origem_cobranca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_charges_status_cobranca_Status",
                        column: x => x.Status,
                        principalTable: "status_cobranca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_charges_tipo_cobranca_Tipo",
                        column: x => x.Tipo,
                        principalTable: "tipo_cobranca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CpfCnpj = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: true),
                    AtendenteId = table.Column<Guid>(type: "uuid", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Observacoes = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_customers_tipo_pessoa_Tipo",
                        column: x => x.Tipo,
                        principalTable: "tipo_pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: true),
                    Referencia = table.Column<string>(type: "text", nullable: true),
                    ComissaoPercentual = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Indicacao = table.Column<bool>(type: "boolean", nullable: false),
                    FonteIndicacao = table.Column<string>(type: "text", nullable: true),
                    capacidades = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_drivers_tipo_prestador_Tipo",
                        column: x => x.Tipo,
                        principalTable: "tipo_prestador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "commissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    RecebedorTipo = table.Column<int>(type: "integer", nullable: false),
                    RecebedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Percentual = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PagoEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Alterada = table.Column<bool>(type: "boolean", nullable: false),
                    JustificativaAlteracao = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_commissions_status_comissao_Status",
                        column: x => x.Status,
                        principalTable: "status_comissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_commissions_tipo_comissao_Tipo",
                        column: x => x.Tipo,
                        principalTable: "tipo_comissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_commissions_tipo_recebedor_comissao_RecebedorTipo",
                        column: x => x.RecebedorTipo,
                        principalTable: "tipo_recebedor_comissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fleets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Categoria = table.Column<int>(type: "integer", nullable: false),
                    Capacidade = table.Column<int>(type: "integer", nullable: false),
                    TemWc = table.Column<bool>(type: "boolean", nullable: false),
                    TemAr = table.Column<bool>(type: "boolean", nullable: false),
                    TemWifi = table.Column<bool>(type: "boolean", nullable: false),
                    TemAntt = table.Column<bool>(type: "boolean", nullable: false),
                    GaragemId = table.Column<Guid>(type: "uuid", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fleets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fleets_categoria_veiculo_Categoria",
                        column: x => x.Categoria,
                        principalTable: "categoria_veiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fleets_tipo_veiculo_Tipo",
                        column: x => x.Tipo,
                        principalTable: "tipo_veiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Prioridade = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Prazo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    VinculoTipo = table.Column<int>(type: "integer", nullable: true),
                    VinculoId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tasks_prioridade_tarefa_Prioridade",
                        column: x => x.Prioridade,
                        principalTable: "prioridade_tarefa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tasks_status_tarefa_Status",
                        column: x => x.Status,
                        principalTable: "status_tarefa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tasks_vinculo_tarefa_VinculoTipo",
                        column: x => x.VinculoTipo,
                        principalTable: "vinculo_tarefa",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "quote_services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoServico = table.Column<int>(type: "integer", nullable: false),
                    Subtipo = table.Column<int>(type: "integer", nullable: false),
                    DataIda = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraIda = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Origem = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Destino = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DataVolta = table.Column<DateOnly>(type: "date", nullable: true),
                    HoraVolta = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    NumPassageiros = table.Column<int>(type: "integer", nullable: false),
                    TipoVeiculoPreferido = table.Column<int>(type: "integer", nullable: true),
                    IdiomaRequerido = table.Column<string>(type: "text", nullable: true),
                    Observacoes = table.Column<string>(type: "text", nullable: true),
                    caracteristicas = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quote_services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quote_services_quotes_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_quote_services_subtipo_servico_Subtipo",
                        column: x => x.Subtipo,
                        principalTable: "subtipo_servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_quote_services_tipo_servico_TipoServico",
                        column: x => x.TipoServico,
                        principalTable: "tipo_servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_quote_services_tipo_veiculo_TipoVeiculoPreferido",
                        column: x => x.TipoVeiculoPreferido,
                        principalTable: "tipo_veiculo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: true),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Origem = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Destino = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DataHoraIda = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataHoraVolta = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ValorServico = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Acrescimo = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    ChaveAcessoCheckin = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CheckinEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CheckinLat = table.Column<double>(type: "double precision", nullable: true),
                    CheckinLng = table.Column<double>(type: "double precision", nullable: true),
                    LocalizacaoAtualLat = table.Column<double>(type: "double precision", nullable: true),
                    LocalizacaoAtualLng = table.Column<double>(type: "double precision", nullable: true),
                    InicioServicoEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FimServicoEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_items_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_items_subtipo_servico_Tipo",
                        column: x => x.Tipo,
                        principalTable: "subtipo_servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Rua = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Numero = table.Column<string>(type: "text", nullable: true),
                    Cidade = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Uf = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_customer_addresses_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customer_addresses_tipo_endereco_Tipo",
                        column: x => x.Tipo,
                        principalTable: "tipo_endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver_documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Categoria = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Numero = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    ValidoAte = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driver_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_driver_documents_drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_driver_documents_tipo_documento_prestador_Tipo",
                        column: x => x.Tipo,
                        principalTable: "tipo_documento_prestador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver_languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    Idioma = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Nivel = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driver_languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_driver_languages_drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_driver_languages_nivel_idioma_Nivel",
                        column: x => x.Nivel,
                        principalTable: "nivel_idioma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FleetId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroOrdem = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Placa = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    GaragemId = table.Column<Guid>(type: "uuid", nullable: true),
                    MotoristaHabitualId = table.Column<Guid>(type: "uuid", nullable: true),
                    DisponivelDesde = table.Column<DateOnly>(type: "date", nullable: true),
                    MotivoIndisponibilidade = table.Column<string>(type: "text", nullable: true),
                    LocalizacaoLat = table.Column<double>(type: "double precision", nullable: true),
                    LocalizacaoLng = table.Column<double>(type: "double precision", nullable: true),
                    features = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vehicles_drivers_MotoristaHabitualId",
                        column: x => x.MotoristaHabitualId,
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_vehicles_fleets_FleetId",
                        column: x => x.FleetId,
                        principalTable: "fleets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicles_status_veiculo_Status",
                        column: x => x.Status,
                        principalTable: "status_veiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_charges_OrderId",
                table: "charges",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_charges_Origem",
                table: "charges",
                column: "Origem");

            migrationBuilder.CreateIndex(
                name: "IX_charges_Status",
                table: "charges",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_charges_Tipo",
                table: "charges",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_commissions_OrderItemId",
                table: "commissions",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_commissions_RecebedorTipo_RecebedorId",
                table: "commissions",
                columns: new[] { "RecebedorTipo", "RecebedorId" });

            migrationBuilder.CreateIndex(
                name: "IX_commissions_Status",
                table: "commissions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_commissions_Tipo",
                table: "commissions",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_customer_addresses_CustomerId",
                table: "customer_addresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_customer_addresses_Tipo",
                table: "customer_addresses",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_customers_CpfCnpj",
                table: "customers",
                column: "CpfCnpj");

            migrationBuilder.CreateIndex(
                name: "IX_customers_Email",
                table: "customers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_customers_Nome",
                table: "customers",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_customers_Tipo",
                table: "customers",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_driver_documents_DriverId",
                table: "driver_documents",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_driver_documents_Tipo",
                table: "driver_documents",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_driver_languages_DriverId",
                table: "driver_languages",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_driver_languages_Nivel",
                table: "driver_languages",
                column: "Nivel");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_Nome",
                table: "drivers",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_Referencia",
                table: "drivers",
                column: "Referencia");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_Tipo",
                table: "drivers",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_fleets_Categoria",
                table: "fleets",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_fleets_Tipo",
                table: "fleets",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_OrderId",
                table: "invoices",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invoices_Status",
                table: "invoices",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_order_audit_logs_OrderId",
                table: "order_audit_logs",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_ChaveAcessoCheckin",
                table: "order_items",
                column: "ChaveAcessoCheckin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_items_OrderId",
                table: "order_items",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_Tipo",
                table: "order_items",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_orders_CustomerId",
                table: "orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Origem",
                table: "orders",
                column: "Origem");

            migrationBuilder.CreateIndex(
                name: "IX_orders_QuoteId",
                table: "orders",
                column: "QuoteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_StatusComercial",
                table: "orders",
                column: "StatusComercial");

            migrationBuilder.CreateIndex(
                name: "IX_orders_StatusOperacional",
                table: "orders",
                column: "StatusOperacional");

            migrationBuilder.CreateIndex(
                name: "IX_password_reset_tokens_TokenHash",
                table: "password_reset_tokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quote_services_QuoteId",
                table: "quote_services",
                column: "QuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_quote_services_Subtipo",
                table: "quote_services",
                column: "Subtipo");

            migrationBuilder.CreateIndex(
                name: "IX_quote_services_TipoServico",
                table: "quote_services",
                column: "TipoServico");

            migrationBuilder.CreateIndex(
                name: "IX_quote_services_TipoVeiculoPreferido",
                table: "quote_services",
                column: "TipoVeiculoPreferido");

            migrationBuilder.CreateIndex(
                name: "IX_quotes_CustomerId",
                table: "quotes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_quotes_Origem",
                table: "quotes",
                column: "Origem");

            migrationBuilder.CreateIndex(
                name: "IX_quotes_Status",
                table: "quotes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_Prioridade",
                table: "tasks",
                column: "Prioridade");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_ResponsavelId",
                table: "tasks",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_Status",
                table: "tasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_VinculoTipo_VinculoId",
                table: "tasks",
                columns: new[] { "VinculoTipo", "VinculoId" });

            migrationBuilder.CreateIndex(
                name: "IX_user_sessions_RefreshTokenHash",
                table: "user_sessions",
                column: "RefreshTokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Perfil",
                table: "users",
                column: "Perfil");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_FleetId",
                table: "vehicles",
                column: "FleetId");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_MotoristaHabitualId",
                table: "vehicles",
                column: "MotoristaHabitualId");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_Placa",
                table: "vehicles",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_Status",
                table: "vehicles",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "charges");

            migrationBuilder.DropTable(
                name: "commissions");

            migrationBuilder.DropTable(
                name: "customer_addresses");

            migrationBuilder.DropTable(
                name: "driver_documents");

            migrationBuilder.DropTable(
                name: "driver_languages");

            migrationBuilder.DropTable(
                name: "garages");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "order_audit_logs");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "password_reset_tokens");

            migrationBuilder.DropTable(
                name: "quote_services");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "user_sessions");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "origem_cobranca");

            migrationBuilder.DropTable(
                name: "status_cobranca");

            migrationBuilder.DropTable(
                name: "tipo_cobranca");

            migrationBuilder.DropTable(
                name: "status_comissao");

            migrationBuilder.DropTable(
                name: "tipo_comissao");

            migrationBuilder.DropTable(
                name: "tipo_recebedor_comissao");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "tipo_endereco");

            migrationBuilder.DropTable(
                name: "tipo_documento_prestador");

            migrationBuilder.DropTable(
                name: "nivel_idioma");

            migrationBuilder.DropTable(
                name: "status_fatura");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "quotes");

            migrationBuilder.DropTable(
                name: "subtipo_servico");

            migrationBuilder.DropTable(
                name: "tipo_servico");

            migrationBuilder.DropTable(
                name: "prioridade_tarefa");

            migrationBuilder.DropTable(
                name: "status_tarefa");

            migrationBuilder.DropTable(
                name: "vinculo_tarefa");

            migrationBuilder.DropTable(
                name: "perfil");

            migrationBuilder.DropTable(
                name: "drivers");

            migrationBuilder.DropTable(
                name: "fleets");

            migrationBuilder.DropTable(
                name: "status_veiculo");

            migrationBuilder.DropTable(
                name: "tipo_pessoa");

            migrationBuilder.DropTable(
                name: "status_comercial");

            migrationBuilder.DropTable(
                name: "status_operacional");

            migrationBuilder.DropTable(
                name: "origem_orcamento");

            migrationBuilder.DropTable(
                name: "status_funil");

            migrationBuilder.DropTable(
                name: "tipo_prestador");

            migrationBuilder.DropTable(
                name: "categoria_veiculo");

            migrationBuilder.DropTable(
                name: "tipo_veiculo");
        }
    }
}
