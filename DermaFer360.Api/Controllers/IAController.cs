using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IAController : ControllerBase
    {
        private readonly IIAAplicacao _iaAplicacao;
        private readonly IRelatorioAplicacao _relatorioAplicacao;

        public IAController(IIAAplicacao iaAplicacao, IRelatorioAplicacao relatorioAplicacao)
        {
            _iaAplicacao = iaAplicacao;
            _relatorioAplicacao = relatorioAplicacao;
        }

        [HttpPost("Perguntar")]
        public async Task<IActionResult> Perguntar([FromBody] PerguntaIARequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Pergunta))
            {
                return BadRequest(new { resposta = "Digite uma pergunta." });
            }

            try
            {
                string pergunta = request.Pergunta;
                string contextoFinanceiro = null;

                bool perguntaFinanceira = Regex.IsMatch(pergunta, @"\b(relatório|relatorio|relatórios|relatorios|faturamento|resultado financeiro|receitas do período|despesas do período|lucro do período|lucro em|receitas em|despesas em|quanto faturou|quanto lucrou|total de receitas|total de despesas)\b", RegexOptions.IgnoreCase);

                if (perguntaFinanceira)
                {
                    if (!TentarIdentificarPeriodo(pergunta, out DateTime inicio, out DateTime fim))
                    {
                        return BadRequest(new { resposta = "Para consultar os dados financeiros, informe o período na pergunta. Exemplo: Qual foi o lucro em outubro de 2026?" });
                    }

                    var relatorio = await _relatorioAplicacao.GerarRelatorioAsync(inicio, fim);

                    string receitas = relatorio.TotalReceitas.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
                    string despesas = relatorio.TotalDespesas.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
                    string lucro = relatorio.Lucro.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));

                    string respostaRelatorio =
                        $"Relatório financeiro de {inicio:dd/MM/yyyy} a {fim:dd/MM/yyyy}\n\n" +
                        $"Total de receitas: {receitas}\n" +
                        $"Total de despesas: {despesas}\n" +
                        $"Lucro informado pelo relatório: {lucro}\n\n" +
                        $"Quantidade de receitas: {relatorio.QuantidadeReceitas}\n" +
                        $"Quantidade de despesas: {relatorio.QuantidadeDespesas}";

                    return Ok(new { resposta = respostaRelatorio });
                }

                string resposta = await _iaAplicacao.PerguntarAsync(pergunta, contextoFinanceiro);

                return Ok(new { resposta });
            }
            catch (Exception)
            {
                return StatusCode(500, new { resposta = "Não foi possível processar a pergunta. Verifique se a API, o relatório financeiro e o Ollama estão funcionando." });
            }
        }

        private static bool TentarIdentificarPeriodo(string pergunta, out DateTime inicio, out DateTime fim)
        {
            inicio = default;
            fim = default;

            var datas = Regex.Match(pergunta, @"(?<dia1>\d{1,2})/(?<mes1>\d{1,2})/(?<ano1>\d{4})\s+a\s+(?<dia2>\d{1,2})/(?<mes2>\d{1,2})/(?<ano2>\d{4})", RegexOptions.IgnoreCase);

            if
            (datas.Success && DateTime.TryParseExact(datas.Groups["dia1"].Value + "/" + datas.Groups["mes1"].Value + "/" + datas.Groups["ano1"].Value, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out inicio)
            && DateTime.TryParseExact(datas.Groups["dia2"].Value + "/" + datas.Groups["mes2"].Value + "/" + datas.Groups["ano2"].Value, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fim))
            {
                if (inicio > fim)
                    return false;

                fim = fim.Date.AddDays(1).AddTicks(-1);
                return true;
            }

            var mesAno = Regex.Match(pergunta, @"\b(janeiro|fevereiro|março|marco|abril|maio|junho|julho|agosto|setembro|outubro|novembro|dezembro)\s+de\s+(\d{4})\b", RegexOptions.IgnoreCase);

            if (!mesAno.Success)
                return false;

            string nomeMes = mesAno.Groups[1].Value.ToLowerInvariant();
            int ano = int.Parse(mesAno.Groups[2].Value);

            nomeMes = nomeMes.Replace("marco", "março");

            string[] meses =
            {
                "janeiro", "fevereiro", "março", "abril",
                "maio", "junho", "julho", "agosto",
                "setembro", "outubro", "novembro", "dezembro"
            };

            int mes = Array.IndexOf(meses, nomeMes) + 1;

            if (mes < 1 || ano < 1 || ano > 9998)
                return false;

            inicio = new DateTime(ano, mes, 1);
            fim = inicio.AddMonths(1).AddTicks(-1);

            return true;
        }
    }
    public class PerguntaIARequest
    {
        public string Pergunta { get; set; } = string.Empty;
    }
}