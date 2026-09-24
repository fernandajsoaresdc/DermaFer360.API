
using DermaFer360.Aplicacao.DTO;
using DermaFer360.Aplicacao.Interfaces;


namespace DermaFer360.Aplicacao
{
    public class RelatorioAplicacao : IRelatorioAplicacao
    {
        readonly IReceitaRepositorio _receitaRepositorio;
        readonly IDespesaRepositorio _despesaRepositorio;
        readonly IProdutoRepositorio _produtoRepositorio;

        public RelatorioAplicacao(IReceitaRepositorio receitaRepositorio, IDespesaRepositorio despesaRepositorio, IProdutoRepositorio produtoRepositorio)
        {
            _receitaRepositorio = receitaRepositorio;
            _despesaRepositorio = despesaRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<RelatorioFinanceiroDTO> GerarRelatorioAsync(DateTime dataInicial, DateTime dataFinal)
        {
            var receitas = await _receitaRepositorio.ListarPorPeriodoAsync(dataInicial, dataFinal);
            var despesas = await _despesaRepositorio.ListarPorPeriodoAsync(dataInicial, dataFinal);

            var relatorio = new RelatorioFinanceiroDTO
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal,

                TotalReceitas = receitas.Sum(r => r.Quantidade * r.PrecoUnitario),

                TotalDespesas = despesas.Sum(d => d.Valor),

                Lucro = receitas.Sum(r => r.Quantidade * r.PrecoUnitario) - despesas.Sum(d => d.Valor),

                QuantidadeReceitas = receitas.Count(),

                QuantidadeDespesas = despesas.Count(),

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

                GraficoFinanceiro = receitas
                .GroupBy(r => new { r.DataReceita.Year, r.DataReceita.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new GraficoFinanceiroDTO
                {
                    Mes = $"{g.Key.Month}/{g.Key.Year}",

                    Receita = g.Sum(r => r.Quantidade * r.PrecoUnitario),

                    Despesa = despesas
                    .Where(d => d.DataDespesa.Month == g.Key.Month && d.DataDespesa.Year == g.Key.Year).Sum(d => d.Valor)
                }).ToList()
            };

            return dashboard;
        }
    }
}