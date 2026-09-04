namespace DermaFer360.Dominio.Entidades;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal PrecoCusto { get; set; }
    public decimal PrecoVenda { get; set; }
    public int Estoque { get; set; }
    public bool Ativo { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

}