namespace DermaFer360.Dominio.Entidades;

public class Receita
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public DateTime DataReceita { get; set; }
    public bool Ativo { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
}