using Microsoft.EntityFrameworkCore;
using DermaFer360.Dominio.Entidades;

namespace DataAccess.Repositorio;

public class DespesaRepositorio : BaseRepositorio, IDespesaRepositorio
{
    public DespesaRepositorio(DermaFer360Context context) : base(context)
    {}

    public async Task<int> CriarAsync(Despesa despesa)
    {
        await _context.Despesas.AddAsync(despesa);
        await _context.SaveChangesAsync();

        return despesa.Id;
    }

    public async Task AtualizarAsync(Despesa despesa)
    {
        _context.Despesas.Update(despesa);
        await _context.SaveChangesAsync();
    }

    public async Task<Despesa> ObterAsync(int despesaId)
    {
        return await _context.Despesas
        .Include(d => d.Usuario)
        .Where(d => d.Id == despesaId)
        .Where(d => d.Ativo)
        .FirstOrDefaultAsync();
    }

     public async Task DeleteAsync(Despesa despesa)
    {
        despesa.Ativo = false;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Despesa>> ListarAsync()
    {
        return await _context.Despesas
        .AsNoTracking()
        .Where(d => d.Ativo)
        .OrderBy(d => d.DataDespesa)
        .ToListAsync();
    }

    public async Task<IEnumerable<Despesa>> ListarPorPeriodoAsync(DateTime dataInicial, DateTime dataFinal)
{
    return await _context.Despesas
        .Where(d => d.Ativo && d.DataDespesa >= dataInicial && d.DataDespesa <= dataFinal)
        .OrderByDescending(d => d.DataDespesa)
        .ToListAsync();
}

}