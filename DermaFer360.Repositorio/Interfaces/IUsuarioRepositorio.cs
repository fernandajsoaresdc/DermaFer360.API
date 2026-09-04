using System.Diagnostics;
using DermaFer360.Dominio.Entidades;

public interface IUsuarioRepositorio
{
    Task<int> CriarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task <Usuario> ObterAsync(int usuarioId);
    Task<Usuario> ObterPorEmailAsync(string email);
    Task DeleteAsync(Usuario usuario);
    Task <IEnumerable<Usuario>> ListarAsync();
   
}