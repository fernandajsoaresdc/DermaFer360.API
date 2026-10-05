namespace DermaFer360.Api.Models.Requisicao
{
    public class ProdutoAtualizar
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public bool Ativo { get; set; }
        public string Descricao {get; set; }
        public int Estoque {get; set; }
    }
}