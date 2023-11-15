using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DEPLOY.Cachorro.Repository.Migrations.API
{
    /// <inheritdoc />
    public partial class InitDatabaseAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Adotado = table.Column<bool>(type: "bit", nullable: false),
                    Pelagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tutor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CPF = table.Column<string>(type: "varchar(11)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutor", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cao",
                columns: new[] { "Id", "Adotado", "DataAlteracao", "DataCadastro", "Nascimento", "Nome", "Pelagem", "Peso" },
                values: new object[] { new Guid("a1a53d28-3aad-46d9-99da-3b428152f82d"), true, new DateTime(2023, 11, 13, 21, 37, 45, 450, DateTimeKind.Local).AddTicks(2644), new DateTime(2023, 11, 13, 21, 37, 45, 450, DateTimeKind.Local).AddTicks(2631), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rex", "None", 9.3m });

            migrationBuilder.CreateIndex(
                name: "IX_Cao_Adotado",
                table: "Cao",
                column: "Adotado");

            migrationBuilder.CreateIndex(
                name: "IX_Cao_Id_DataAlteracao",
                table: "Cao",
                columns: new[] { "Id", "DataAlteracao" });

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_Id_DataAlteracao",
                table: "Tutor",
                columns: new[] { "Id", "DataAlteracao" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cao");

            migrationBuilder.DropTable(
                name: "Tutor");
        }
    }
}
