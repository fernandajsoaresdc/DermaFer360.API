namespace DermaFer360.Api.Models.Requisicao
{
    public class DespesaAtualizar
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataDespesa { get; set; }

        public bool Ativo { get; set; }

        public int UsuarioId { get; set; }
    }
}