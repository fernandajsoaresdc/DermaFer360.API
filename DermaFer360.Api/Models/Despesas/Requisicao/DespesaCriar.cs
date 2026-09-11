namespace DermaFer360.Api.Models.Requisicao
{
    public class DespesaCriar
    {
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataDespesa { get; set; }
        public int UsuarioId { get; set; }
    }
}