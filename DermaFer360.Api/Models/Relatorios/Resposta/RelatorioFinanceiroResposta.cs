namespace DermaFer360.Api.Models.Resposta
{
    public class RelatorioFinanceiroResposta
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Lucro { get; set; }
        public int QuantidadeReceitas { get; set; }
    }
}