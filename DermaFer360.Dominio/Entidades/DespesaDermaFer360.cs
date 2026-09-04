namespace DermaFer360.Dominio.Entidades;

public class Despesa
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataDespesa { get; set; }
    public bool Ativo { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
    public int? ProdutoId { get; set; }
    public Produto Produto { get; set; }
}