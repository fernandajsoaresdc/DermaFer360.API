using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DermaFer360.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class AjustarRelacaoProdutoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Usuarios_UsuarioId1",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_UsuarioId1",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Produtos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId1",
                table: "Produtos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_UsuarioId1",
                table: "Produtos",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Usuarios_UsuarioId1",
                table: "Produtos",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId");
        }
    }
}
