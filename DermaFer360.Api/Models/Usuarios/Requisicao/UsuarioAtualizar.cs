using System.ComponentModel.DataAnnotations;
using DermaFer360.Dominio.Enumeradores;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DermaFer360.Api.Models.Requisicao
{
    public class UsuarioAtualizar
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; }

       [Required, EmailAddress, StringLength(254)]
        public string Email { get; set; }
        public CargoFinanceiro Cargo { get; set; }
        public bool Ativo { get; set; }
    }
}