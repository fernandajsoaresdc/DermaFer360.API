using DermaFer360.Aplicacao.DTOs;
using DermaFer360.Dominio.Entidades;

namespace DermaFer360.Aplicacao.Interfaces
{
    public interface IUsuarioAplicacao
    {
        Task<int> CriarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task AlterarSenhaAsync(Usuario usuario);
        Task<Usuario> ObterAsync(int usuarioId);
        Task<Usuario> ObterPorEmailAsync(string email);
        Task DeletarAsync(int usuarioId);
        Task RestaurarAsync(int usuarioId);
        Task<IEnumerable<Usuario>> ListarAsync();
        IEnumerable<CargoUsuario> ListarCargos();
        

    }
}