namespace DermaFer360.Aplicacao.DTO;

public class ReceitaDTO
{
    public string Produto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataReceita {get; set; }
}