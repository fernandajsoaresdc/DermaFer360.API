using Microsoft.EntityFrameworkCore;
using DermaFer360.Dominio.Entidades;

namespace DataAccess.Repositorio;

public class ProdutoRepositorio : BaseRepositorio, IProdutoRepositorio
{
    public ProdutoRepositorio(DermaFer360Context context) : base(context)
    {}

    public async Task<int> CriarAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();

        return produto.Id;
    }

    public async Task AtualizarAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task<Produto> ObterAsync(int produtoId)
    {
        return await _context.Produtos
        .Include(p => p.Usuario)
        .Where(p => p.Id == produtoId)
        .Where(p => p.Ativo)
        .FirstOrDefaultAsync();
    }

     public async Task DeleteAsync(Produto produto)
    {
        produto.Ativo = false;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Produto>> ListarAsync()
    {
        return await _context.Produtos
        .AsNoTracking()
        .Where(u => u.Ativo)
        .OrderBy(u => u.Nome)
        .ToListAsync();
    }

}