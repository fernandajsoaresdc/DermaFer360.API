using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Service
{
    public class IAAplicacao : IIAAplicacao
    {
        private readonly HttpClient _httpClient;
        private readonly IRelatorioAplicacao _relatorioAplicacao;

        private static readonly string[] Meses =
        {
            "janeiro", "fevereiro", "março", "abril",
            "maio", "junho", "julho", "agosto",
            "setembro", "outubro", "novembro", "dezembro"
        };

        public IAAplicacao(HttpClient httpClient,IRelatorioAplicacao relatorioAplicacao)
        {
            _httpClient = httpClient;
            _relatorioAplicacao = relatorioAplicacao;
        }

        public async Task<string> PerguntarAsync(string pergunta,string contextoFinanceiro = null)
        {
            if (string.IsNullOrWhiteSpace(pergunta))
                return "Digite uma pergunta.";

            string mensagemUsuario = pergunta;

     
            bool perguntaFinanceira = Regex.IsMatch(mensagemUsuario,@"\b(relatório|relatorio|relatórios|relatorios|faturamento|resultado financeiro|receitas?|despesas?|lucro|prejuízo|prejuizo|margem de lucro|fluxo de caixa|quanto faturou|quanto lucrou|quanto vendeu|total financeiro)\b",RegexOptions.IgnoreCase);

            if (perguntaFinanceira)
            {
                if (!TentarIdentificarPeriodo(mensagemUsuario,out DateTime inicio,out DateTime fim))
                {
                    return "Para consultar os dados financeiros, informe o período na pergunta. Exemplo: Qual foi o lucro em outubro de 2026?";
                }

                var relatorio = await _relatorioAplicacao.GerarRelatorioAsync(inicio, fim);

                string receitas = relatorio.TotalReceitas.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));

                string despesas = relatorio.TotalDespesas.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));

                string lucro = relatorio.Lucro.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));

                string dadosFinanceiros = $"""
                    RELATÓRIO OFICIAL DO DERMAFER360

                    Período: {inicio:dd/MM/yyyy} a {fim:dd/MM/yyyy}

                    Total de receitas: {receitas}
                    Total de despesas: {despesas}
                    Lucro informado pelo relatório: {lucro}

                    Quantidade de receitas: {relatorio.QuantidadeReceitas}
                    Quantidade de despesas: {relatorio.QuantidadeDespesas}
                    """;

                mensagemUsuario += $"""

                    {dadosFinanceiros}

                    Analise exclusivamente os dados oficiais apresentados.
                    Responda à pergunta utilizando os valores do relatório.
                    Não invente informações.
                    """;
            }
            else if (!string.IsNullOrWhiteSpace(contextoFinanceiro))
            {
                mensagemUsuario += "\n\nContexto financeiro:\n" + contextoFinanceiro;
            }

            
            var instrucao = """
                # PAPEL

                Você é a DermaFer IA, assistente financeira do sistema
                DermaFer360, utilizado pelo setor financeiro de uma
                empresa de dermocosméticos.

                # IDIOMA E ESTILO

                - Responda sempre em português do Brasil.
                - Utilize linguagem profissional, didática e objetiva.
                - Prefira respostas curtas e claras.
                - Utilize valores monetários no formato brasileiro.
                - Apresente fórmulas quando realizar cálculos.

                # CONCEITOS FINANCEIROS

                - Receita: valor obtido com vendas ou serviços.
                - Custo: gasto para fabricar ou adquirir produtos.
                - Despesa: gasto administrativo ou operacional.
                - Lucro bruto: receita menos custos dos produtos vendidos.
                - Lucro líquido: receita menos custos e despesas.
                - Margem de lucro: (lucro / receita) x 100.
                - Markup: índice aplicado sobre o custo para formar
                  o preço de venda.

                Nunca confunda receita, custo, despesa, lucro e margem.

                # DADOS OFICIAIS

                - Utilize exclusivamente os dados oficiais quando
                  receber um relatório do DermaFer360.
                - Nunca invente receitas, despesas ou lucros.
                - Não altere os valores fornecidos.
                - Se a informação não estiver disponível, informe isso.
                - Não afirme que consultou o banco quando não ocorreu
                  uma consulta.
                - Diferencie dados reais de exemplos ilustrativos.

                # RESPOSTAS

                - Perguntas conceituais: explique o conceito e,
                  quando necessário, apresente exemplo ilustrativo.
                - Perguntas financeiras: utilize os dados oficiais.
                - Cálculos: apresente fórmula, valores e resultado.
                - Comparações: destaque diferenças e variações.

                # LIMITES

                - Apoie decisões financeiras, sem substituir
                  contador, auditor ou consultor tributário.
                - Em questões fiscais, recomende validação profissional.
                - Não responda assuntos fora de finanças e do
                  sistema DermaFer360.
                - Nunca solicite ou repita senhas e tokens.

                # OBJETIVO

                Apoiar a governança financeira do DermaFer360,
                fornecendo informações claras, corretas e verificáveis.
                """;

            
            var requisicao = new
            {
                model = "phi3:mini",
                stream = false,
                keep_alive = "30m",
                options = new
                {
                    num_predict = 250,
                    num_ctx = 2048,
                    temperature = 0.2
                },
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = instrucao
                    },
                    new
                    {
                        role = "user",
                        content = mensagemUsuario
                    }
                }
            };

            
            var resposta = await _httpClient.PostAsJsonAsync("api/chat",requisicao);

            resposta.EnsureSuccessStatusCode();

            var json = await resposta.Content.ReadFromJsonAsync<OllamaResposta>();

            return json?.Message?.Content?? "Não foi possível obter uma resposta da DermaFer IA.";
        }

       
        private static bool TentarIdentificarPeriodo(string pergunta,out DateTime inicio,out DateTime fim)
        {
            inicio = default;
            fim = default;

            var datas = Regex.Match(pergunta, @"(?<dia1>\d{1,2})/(?<mes1>\d{1,2})/(?<ano1>\d{4})\s+a\s+(?<dia2>\d{1,2})/(?<mes2>\d{1,2})/(?<ano2>\d{4})", RegexOptions.IgnoreCase);

            if (datas.Success)
            {
                bool dataInicialValida = DateTime.TryParseExact(
                    datas.Groups["dia1"].Value + "/" +
                    datas.Groups["mes1"].Value + "/" +
                    datas.Groups["ano1"].Value,
                    "d/M/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out inicio
                );

                bool dataFinalValida = DateTime.TryParseExact(
                    datas.Groups["dia2"].Value + "/" +
                    datas.Groups["mes2"].Value + "/" +
                    datas.Groups["ano2"].Value,
                    "d/M/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fim
                );

                if (!dataInicialValida || !dataFinalValida || inicio.Date > fim.Date)
                    return false;

                inicio = inicio.Date;
                fim = fim.Date.AddDays(1).AddTicks(-1);

                return true;
            }

            
            var mesAno = Regex.Match(pergunta, @"\b(janeiro|fevereiro|março|marco|abril|maio|junho|julho|agosto|setembro|outubro|novembro|dezembro)\s+de\s+(\d{4})\b", RegexOptions.IgnoreCase);

            if (!mesAno.Success)
                return false;

            string nomeMes = mesAno.Groups[1].Value
                .ToLowerInvariant()
                .Replace("marco", "março");

            if (!int.TryParse(mesAno.Groups[2].Value, out int ano))
                return false;

            int mes = Array.IndexOf(Meses, nomeMes) + 1;

            if (mes < 1 || ano < 1 || ano > 9998)
                return false;

            inicio = new DateTime(ano, mes, 1);

            fim = inicio
                .AddMonths(1)
                .AddTicks(-1);

            return true;
        }

        private class OllamaResposta
        {
            [JsonPropertyName("message")]
            public OllamaMensagem Message { get; set; }
        }

        private class OllamaMensagem
        {
            [JsonPropertyName("role")]
            public string Role { get; set; }

            [JsonPropertyName("content")]
            public string Content { get; set; }
        }
    }
}
