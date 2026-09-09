using DermaFer360.Dominio.Enumeradores;

namespace DermaFer360.Dominio.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public CargoFinanceiro Cargo { get; set; }
        public bool Ativo { get; set; }
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();



        public Usuario()
        {
            Ativo = true;
        }

        public void Deletar()
        {
            Ativo = false;
        }

        public void Restaurar()
        {
            Ativo = true;
        }
    }
}