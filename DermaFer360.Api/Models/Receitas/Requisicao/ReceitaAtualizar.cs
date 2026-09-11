namespace DermaFer360.Api.Models.Requisicao
{
    public class ReceitaAtualizar
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataReceita { get; set; }
        public bool Ativo { get; set; }
        public int UsuarioId { get; set;}
        
    }
}