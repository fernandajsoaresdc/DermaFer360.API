using DermaFer360.Aplicacao.DTO;

namespace DermaFer360.Aplicacao.Interfaces
{
    public interface IAutenticacaoAplicacao
    {
        Task<Login> LoginAsync(LoginAutenticacao login);
    }
}