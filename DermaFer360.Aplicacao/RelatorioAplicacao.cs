
using DermaFer360.Aplicacao.DTO;
using DermaFer360.Aplicacao.Interfaces;


namespace DermaFer360.Aplicacao
{
    public class RelatorioAplicacao : IRelatorioAplicacao
    {
        readonly IReceitaRepositorio _receitaRepositorio;
        readonly IDespesaRepositorio _despesaRepositorio;
        readonly IProdutoRepositorio _produtoRepositorio;
        readonly IRelatorioRepositorio _relatorioRepositorio;

        public RelatorioAplicacao(IReceitaRepositorio receitaRepositorio, IDespesaRepositorio despesaRepositorio, IProdutoRepositorio produtoRepositorio, IRelatorioRepositorio relatorioRepositorio)
        {
            _receitaRepositorio = receitaRepositorio;
            _despesaRepositorio = despesaRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _relatorioRepositorio = relatorioRepositorio;
        }

        public async Task<RelatorioFinanceiroDTO> GerarRelatorioAsync(DateTime dataInicial, DateTime dataFinal)
        {
            var resumo = await _relatorioRepositorio.ObterResumoAsync(dataInicial, dataFinal);
            var receitas = await _receitaRepositorio.ListarPorPeriodoAsync(dataInicial, dataFinal);
            var despesas = await _despesaRepositorio.ListarPorPeriodoAsync(dataInicial, dataFinal);

            var relatorio = new RelatorioFinanceiroDTO
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                TotalReceitas = resumo.TotalReceitas,
                TotalDespesas = resumo.TotalDespesas,
                Lucro = resumo.Lucro,
                QuantidadeReceitas = resumo.QuantidadeReceitas,
                QuantidadeDespesas = resumo.QuantidadeDespesas,

                Receitas = receitas.Select(r => new ReceitaDTO
                {
                    Produto = r.Produto.Nome,
                    Quantidade = r.Quantidade,
                    PrecoUnitario = r.PrecoUnitario,
                    ValorTotal = r.Quantidade * r.PrecoUnitario,
                    DataReceita = r.DataReceita
                }).ToList(),

                Despesas = despesas.Select(d => new DespesaDTO
                {
                    Descricao = d.Descricao,
                    Valor = d.Valor,
                    DataDespesa = d.DataDespesa
                }).ToList()

            };

            return relatorio;
        }

        public async Task<DashboardDTO> DashboardAsync(DateTime dataInicial, DateTime dataFinal)
        {
            var receitas = await _receitaRepositorio.ListarPorPeriodoAsync(dataInicial, dataFinal);
            var despesas = await _despesaRepositorio.ListarPorPeriodoAsync(dataInicial, dataFinal);
            var totalProdutos = await _produtoRepositorio.TotalProdutosAsync();

            decimal receitaTotal = receitas.Sum(r => r.Quantidade * r.PrecoUnitario);

            decimal despesaTotal = despesas.Sum(d => d.Valor);

            decimal lucroTotal = receitaTotal - despesaTotal;


            var graficoFinanceiro = new List<GraficoFinanceiroDTO>();

            var inicio = dataInicial.Date;
            var fim = dataFinal.Date;

            var quantidadeDias = (fim - inicio).Days;

            if (quantidadeDias <= 31)
            {
                for (var data = inicio; data <= fim; data = data.AddDays(1))
                {
                    var receita = receitas
                        .Where(r => r.DataReceita.Date == data)
                        .Sum(r => r.Quantidade * r.PrecoUnitario);

                    var despesa = despesas
                        .Where(d => d.DataDespesa.Date == data)
                        .Sum(d => d.Valor);

                    graficoFinanceiro.Add(new GraficoFinanceiroDTO
                    {
                        Mes = data.ToString("dd/MM/yyyy"),
                        Receita = receita,
                        Despesa = despesa
                    });
                }
            }
            else
            {
                var primeiroMes = new DateTime(inicio.Year, inicio.Month, 1);
                var ultimoMes = new DateTime(fim.Year, fim.Month, 1);

                for (
                    var mes = primeiroMes;
                    mes <= ultimoMes;
                    mes = mes.AddMonths(1)
                )
                {
                    var inicioMes = mes > inicio
                        ? mes
                        : inicio;

                    var fimMes = mes.AddMonths(1).AddDays(-1) < fim
                        ? mes.AddMonths(1).AddDays(-1)
                        : fim;

                    var receita = receitas
                        .Where(r =>
                            r.DataReceita.Date >= inicioMes &&
                            r.DataReceita.Date <= fimMes
                        )
                        .Sum(r => r.Quantidade * r.PrecoUnitario);

                    var despesa = despesas
                        .Where(d =>
                            d.DataDespesa.Date >= inicioMes &&
                            d.DataDespesa.Date <= fimMes
                        )
                        .Sum(d => d.Valor);

                    graficoFinanceiro.Add(new GraficoFinanceiroDTO
                    {
                        Mes = $"{mes.Month:D2}/{mes.Year}",
                        Receita = receita,
                        Despesa = despesa
                    });
                }
            }

            var dashboard = new DashboardDTO
            {
                ReceitaTotal = receitaTotal,
                DespesaTotal = despesaTotal,
                LucroTotal = lucroTotal,
                ProdutosEstoque = totalProdutos,

                UltimasReceitas = receitas
                .OrderByDescending(r => r.DataReceita)
                .Select(r => new ReceitaDTO
                {
                    Produto = r.Produto.Nome,
                    Quantidade = r.Quantidade,
                    PrecoUnitario = r.PrecoUnitario,
                    ValorTotal = r.Quantidade * r.PrecoUnitario,
                    DataReceita = r.DataReceita

                }).ToList(),

                GraficoFinanceiro = graficoFinanceiro
            };

            return dashboard;
        }
    }
}