using Microsoft.EntityFrameworkCore;
using DermaFer360.Dominio.Entidades;

namespace DataAccess.Repositorio;

public class ReceitaRepositorio : BaseRepositorio, IReceitaRepositorio
{
    public ReceitaRepositorio(DermaFer360Context context) : base(context)
    {}

    public async Task<int> CriarAsync(Receita receita)
    {
        await _context.Receitas.AddAsync(receita);
        await _context.SaveChangesAsync();

        return receita.Id;
    }

    public async Task AtualizarAsync(Receita receita)
    {
        _context.Receitas.Update(receita);
        await _context.SaveChangesAsync();
    }

    public async Task<Receita> ObterAsync(int receitaId)
    {
        return await _context.Receitas
        .Include(r => r.Produto)
        .Include(r => r.Usuario)
        .Where(r => r.Id == receitaId)
        .Where(r => r.Ativo)
        .FirstOrDefaultAsync();
    }

     public async Task DeleteAsync(Receita receita)
    {
        receita.Ativo = false;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Receita>> ListarAsync()
    {
        return await _context.Receitas
        .AsNoTracking()
        .Include(r => r.Produto)
        .Include(r => r.Usuario)
        .Where(r => r.Ativo)
        .OrderBy(r => r.DataReceita)
        .ToListAsync();
    }

    public async Task<IEnumerable<Receita>> ListarPorPeriodoAsync(DateTime dataInicial, DateTime dataFinal)
    {
        return await _context.Receitas
        .Include(r => r.Produto)
        .Where(r => r.Ativo && r.DataReceita >= dataInicial && r.DataReceita <= dataFinal)
        .OrderByDescending(r => r.DataReceita)
        .ToListAsync();
    }

}