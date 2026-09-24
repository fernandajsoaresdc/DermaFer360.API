namespace DermaFer360.Aplicacao.DTO;

public class DespesaDTO
{
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataDespesa { get; set; }
}