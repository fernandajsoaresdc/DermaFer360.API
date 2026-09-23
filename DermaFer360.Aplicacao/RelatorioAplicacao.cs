
using DermaFer360.Aplicacao.DTO;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Aplicacao
{
    public class RelatorioAplicacao : IRelatorioAplicacao
    {
        readonly IReceitaRepositorio _receitaRepositorio;
        readonly IDespesaRepositorio _despesaRepositorio;

        public RelatorioAplicacao(IReceitaRepositorio receitaRepositorio, IDespesaRepositorio despesaRepositorio)
        {
            _receitaRepositorio = receitaRepositorio;
            _despesaRepositorio = despesaRepositorio;
        }

        public async Task<RelatorioFinanceiroDTO> GerarRelatorioAsync(DateTime dataInicial, DateTime dataFinal)
        {
            var receitas = await _receitaRepositorio.ListarPorPeriodoAsync(dataInicial, dataFinal);
            var despesas = await _despesaRepositorio.ListarPorPeriodoAsync(dataInicial, dataFinal);

            var totalReceitas = receitas.Sum(r => r.Quantidade * r.PrecoUnitario);
            var totalDespesas = despesas.Sum(d => d.Valor);

            return new RelatorioFinanceiroDTO
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Lucro = totalReceitas - totalDespesas,
                QuantidadeReceitas = receitas.Count(),
                QuantidadeDespesas = despesas.Count()
            };
        }
    }
}