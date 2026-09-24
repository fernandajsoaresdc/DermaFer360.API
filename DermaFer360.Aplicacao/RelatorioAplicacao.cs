
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
            var relatorio = new RelatorioFinanceiroDTO
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal,

                TotalReceitas = receitas.Sum(r => r.ValorTotal),

                TotalDespesas = despesas.Sum(d => d.Valor),

                Lucro = receitas.Sum(r => r.ValorTotal) - despesas.Sum(d => d.Valor),

                QuantidadeReceitas = receitas.Count(),

                QuantidadeDespesas = despesas.Count(),

                Receitas = receitas.Select(r => new ReceitaDTO
                {
                    Produto = r.Produto.Nome,
                    Quantidade = r.Quantidade,
                    PrecoUnitario = r.PrecoUnitario,
                    ValorTotal = r.ValorTotal,
                    DataReceita = r.DataReceita
                }).ToList(),

                Despesas = despesas.Select(d => new DespesaDTO
                {
                    Descricao = d.Descricao,
                    Valor = d.Valor,
                    DataDespesa = d.DataDespesa
                }).ToList(),

            };

            return relatorio;
        }
    }
}