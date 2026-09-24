using DermaFer360.Dominio.Entidades;
using DermaFer360.Aplicacao.DTO;

namespace DermaFer360.Aplicacao.Interfaces
{
    public interface IRelatorioAplicacao
    {
        Task<RelatorioFinanceiroDTO> GerarRelatorioAsync(DateTime dataInicial, DateTime dataFinal);
         Task<DashboardDTO> DashboardAsync(DateTime dataInicial, DateTime dataFinal);
    }
}