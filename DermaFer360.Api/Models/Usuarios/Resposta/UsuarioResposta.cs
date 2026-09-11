using DermaFer360.Dominio.Enumeradores;

namespace DermaFer360.Api.Models.Resposta
{
    public class UsuarioResposta
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public CargoFinanceiro Cargo { get; set; }
        public bool Ativo { get; set; }
    }
}

