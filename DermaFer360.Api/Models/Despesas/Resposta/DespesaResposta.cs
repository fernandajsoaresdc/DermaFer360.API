namespace DermaFer360.Api.Models.Resposta
{
    public class DespesaResposta
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public Decimal Valor { get; set; }
        public DateTime DataDespesa { get; set; }
        public int UsuarioId { get; set; }
        public bool Ativo { get; set; }
    }
}