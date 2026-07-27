using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetExecutive.Infrastructure.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class AddOrcamentosPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuoteId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AtendenteId = table.Column<Guid>(type: "uuid", nullable: true),
                    Origem = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StatusComercial = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StatusOperacional = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "quotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AtendenteId = table.Column<Guid>(type: "uuid", nullable: true),
                    Origem = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: true),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: true),
                    Tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "quote_services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoServico = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Subtipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DataIda = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraIda = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Origem = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Destino = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DataVolta = table.Column<DateOnly>(type: "date", nullable: true),
                    HoraVolta = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    NumPassageiros = table.Column<int>(type: "integer", nullable: false),
                    TipoVeiculoPreferido = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
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
                });

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
                name: "IX_orders_CustomerId",
                table: "orders",
                column: "CustomerId");

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
                name: "IX_quote_services_QuoteId",
                table: "quote_services",
                column: "QuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_quotes_CustomerId",
                table: "quotes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_quotes_Status",
                table: "quotes",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_audit_logs");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "quote_services");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "quotes");
        }
    }
}
