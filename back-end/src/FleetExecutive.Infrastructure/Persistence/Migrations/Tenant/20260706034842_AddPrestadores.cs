using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetExecutive.Infrastructure.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class AddPrestadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "driver_documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "driver_languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    Idioma = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Nivel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_MotoristaHabitualId",
                table: "vehicles",
                column: "MotoristaHabitualId");

            migrationBuilder.CreateIndex(
                name: "IX_driver_documents_DriverId",
                table: "driver_documents",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_driver_languages_DriverId",
                table: "driver_languages",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_Nome",
                table: "drivers",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_Referencia",
                table: "drivers",
                column: "Referencia");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_drivers_MotoristaHabitualId",
                table: "vehicles",
                column: "MotoristaHabitualId",
                principalTable: "drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_drivers_MotoristaHabitualId",
                table: "vehicles");

            migrationBuilder.DropTable(
                name: "driver_documents");

            migrationBuilder.DropTable(
                name: "driver_languages");

            migrationBuilder.DropTable(
                name: "drivers");

            migrationBuilder.DropIndex(
                name: "IX_vehicles_MotoristaHabitualId",
                table: "vehicles");
        }
    }
}
