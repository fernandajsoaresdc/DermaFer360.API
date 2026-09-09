using DermaFer360.Dominio.Entidades;

namespace DermaFer360.Aplicacao.Interfaces
{
    public interface IProdutoAplicacao
    {
        Task<int> CriarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task<Produto> ObterAsync(int produtoId);
        Task DeletarAsync(int produtoId);
        Task<IEnumerable<Produto>> ListarAsync();

    }
}