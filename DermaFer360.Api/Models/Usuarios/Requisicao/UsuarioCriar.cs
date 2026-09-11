using DermaFer360.Dominio.Enumeradores;

namespace DermaFer360.Api.Models.Requisicao
{
    public class UsuarioCriar
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public CargoFinanceiro Cargo { get; set; }
    }
}