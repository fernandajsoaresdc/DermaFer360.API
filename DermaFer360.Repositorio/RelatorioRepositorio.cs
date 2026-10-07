using DermaFer360.Dominio.Dtos;
using Microsoft.EntityFrameworkCore;

public class RelatorioRepositorio : IRelatorioRepositorio
{
    private readonly DermaFer360Context _context; 

    public RelatorioRepositorio(DermaFer360Context context)
    {
        _context = context;
    }

    public async Task<RelatorioResumo> ObterResumoAsync(DateTime dataInicial, DateTime dataFinal)
    {
        var resultado = await _context.Database
            .SqlQuery<RelatorioResumo>($"EXEC sp_RelatorioFinanceiro {dataInicial}, {dataFinal}")
            .ToListAsync();

        return resultado.First();
    }
}