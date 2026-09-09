
namespace DermaFer360.Aplicacao.DTO;

public class RelatorioFinanceiroDTO
{
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Lucro { get; set; }
    public int QuantidadeReceitas { get; set; }
    public int QuantidadeDespesas { get; set;}

}