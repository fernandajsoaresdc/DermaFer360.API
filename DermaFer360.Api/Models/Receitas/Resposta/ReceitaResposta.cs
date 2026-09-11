namespace DermaFer360.Api.Models.Resposta
{
    public class ReceitaResposta
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public DateTime DataReceita { get; set; }
        public int UsuarioId { get; set; }
        public bool Ativo { get; set; }
    }
}