namespace DermaFer360.Aplicacao.DTO;

public class DashboardDTO
{
    public decimal ReceitaTotal { get; set; }
    public decimal DespesaTotal {get; set; }
    public decimal LucroTotal { get; set; }
    public int ProdutosEstoque { get; set; }

     public List<GraficoFinanceiroDTO> GraficoFinanceiro { get; set; } = [];
    public List<ReceitaDTO> UltimasReceitas { get; set; } = [];
}