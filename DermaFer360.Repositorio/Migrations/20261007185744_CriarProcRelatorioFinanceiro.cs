using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DermaFer360.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class CriarProcRelatorioFinanceiro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.Sql(@"
        CREATE PROCEDURE sp_RelatorioFinanceiro
            @DataInicial DATE,
            @DataFinal DATE
        AS
        BEGIN
            SET NOCOUNT ON;

            DECLARE @TotalReceitas DECIMAL(18,2), @QtdReceitas INT;
            DECLARE @TotalDespesas DECIMAL(18,2), @QtdDespesas INT;

            SELECT @TotalReceitas = ISNULL(SUM(Quantidade * PrecoUnitario), 0),
                   @QtdReceitas = COUNT(*)
            FROM Receitas
            WHERE Ativo = 1
              AND DataReceita >= @DataInicial
              AND DataReceita < DATEADD(DAY, 1, @DataFinal);

            SELECT @TotalDespesas = ISNULL(SUM(Valor), 0),
                   @QtdDespesas = COUNT(*)
            FROM Despesas
            WHERE Ativo = 1
              AND DataDespesa >= @DataInicial
              AND DataDespesa < DATEADD(DAY, 1, @DataFinal);

            SELECT @TotalReceitas AS TotalReceitas,
                   @TotalDespesas AS TotalDespesas,
                   @TotalReceitas - @TotalDespesas AS Lucro,
                   @QtdReceitas AS QuantidadeReceitas,
                   @QtdDespesas AS QuantidadeDespesas;
        END");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE sp_RelatorioFinanceiro");
        }
    }
}
