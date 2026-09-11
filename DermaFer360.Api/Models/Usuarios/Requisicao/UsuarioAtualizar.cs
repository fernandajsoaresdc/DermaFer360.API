using DermaFer360.Dominio.Enumeradores;

namespace DermaFer360.Api.Models.Requisicao
{
    public class UsuarioAtualizar
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public CargoFinanceiro Cargo { get; set; }
        public bool Ativo { get; set; }
    }
}