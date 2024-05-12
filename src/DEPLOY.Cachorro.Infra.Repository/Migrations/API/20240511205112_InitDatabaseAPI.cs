using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DEPLOY.Cachorro.Infra.Repository.Migrations.API
{
    /// <inheritdoc />
    public partial class InitDatabaseAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tutor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CPF = table.Column<string>(type: "varchar(11)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cachorro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime", nullable: false),
                    Adotado = table.Column<bool>(type: "bit", nullable: false),
                    Pelagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    TutorId = table.Column<long>(type: "bigint", nullable: true),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    DataAlteracao = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cachorro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cachorro_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Tutor",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Cachorro",
                columns: new[] { "Id", "Adotado", "DataCadastro", "DataNascimento", "Nome", "Pelagem", "Peso", "TutorId" },
                values: new object[] { new Guid("03d4a3a9-31bc-4dd9-89ac-a5ffa22cde8f"), false, new DateTime(2024, 5, 11, 17, 51, 11, 415, DateTimeKind.Local).AddTicks(5820), new DateTime(2024, 5, 11, 17, 51, 11, 415, DateTimeKind.Local).AddTicks(5852), "Rex", "Curto", 10.3m, null });

            migrationBuilder.CreateIndex(
                name: "IX_Cachorro_Adotado",
                table: "Cachorro",
                column: "Adotado");

            migrationBuilder.CreateIndex(
                name: "IX_Cachorro_Id_DataAlteracao",
                table: "Cachorro",
                columns: new[] { "Id", "DataAlteracao" });

            migrationBuilder.CreateIndex(
                name: "IX_Cachorro_TutorId",
                table: "Cachorro",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_Id_DataAlteracao",
                table: "Tutor",
                columns: new[] { "Id", "DataAlteracao" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cachorro");

            migrationBuilder.DropTable(
                name: "Tutor");
        }
    }
}
