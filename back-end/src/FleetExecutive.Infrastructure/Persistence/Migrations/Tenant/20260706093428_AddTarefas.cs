using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetExecutive.Infrastructure.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class AddTarefas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Prioridade = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Prazo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    VinculoTipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    VinculoId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.Id);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tasks");
        }
    }
}
