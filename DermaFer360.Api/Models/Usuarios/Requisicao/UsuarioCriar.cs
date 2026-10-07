using System.ComponentModel.DataAnnotations;
using DermaFer360.Dominio.Enumeradores;

namespace DermaFer360.Api.Models.Requisicao
{
    public class UsuarioCriar
    {
        [Required, StringLength(100)]
        public string Nome { get; set; }

        [Required, EmailAddress,StringLength(254)]
        public string Email { get; set; }

        [Required, StringLength(100, MinimumLength = 6)]
        public string Senha { get; set; }
        public CargoFinanceiro Cargo { get; set; }
    }
}