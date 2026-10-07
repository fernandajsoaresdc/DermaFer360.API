namespace DermaFer360.Dominio.Dtos;

public class RelatorioResumo
{
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Lucro { get; set; }
    public int QuantidadeReceitas { get; set; }
    public int QuantidadeDespesas { get; set; }
}