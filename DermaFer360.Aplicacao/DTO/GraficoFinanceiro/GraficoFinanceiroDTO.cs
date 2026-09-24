namespace DermaFer360.Aplicacao.DTO;

public class GraficoFinanceiroDTO
{
    public string Mes { get; set; } = string.Empty;
    public decimal Receita { get; set; }
    public decimal Despesa { get; set; }
}