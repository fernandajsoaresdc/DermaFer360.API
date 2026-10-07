using DermaFer360.Dominio.Dtos;

public interface IRelatorioRepositorio
{
    Task<RelatorioResumo> ObterResumoAsync(DateTime dataInicial, DateTime dataFinal);
}