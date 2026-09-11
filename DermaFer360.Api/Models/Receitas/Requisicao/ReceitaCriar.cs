namespace DermaFer360.Api.Models.Requisicao
{
    public class ReceitaCriar
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public DateTime DataReceita { get; set; }
        public int UsuarioId { get; set; }

    }
}