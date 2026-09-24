using System.Diagnostics;
using DermaFer360.Dominio.Entidades;

public interface IProdutoRepositorio
{
    Task<int> CriarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
    Task<Produto> ObterAsync(int produtoId);
    Task DeleteAsync(Produto produto);
    Task<int> TotalProdutosAsync();
    Task<IEnumerable<Produto>> ListarAsync();

}


