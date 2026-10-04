namespace DermaFer360.Api.Models.Requisicao
{
    public class ProdutoCriar
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set;}
        public int UsuarioId { get; set; }
        public int Estoque { get; set; }

    }
}