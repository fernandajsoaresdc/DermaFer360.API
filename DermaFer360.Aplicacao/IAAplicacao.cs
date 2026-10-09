
using System.Net.Http.Json;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Aplicacao
{
    public class IAAplicacao : IIAAplicacao
    {
        private readonly HttpClient _httpClient;

        public IAAplicacao(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> PerguntarAsync(string pergunta, string? contextoFinanceiro = null)
        {
            var instrucao = """
            Você é a DermaFer IA, assistente financeira do sistema DermaFer360.

            Responda sempre em português do Brasil, com clareza,
            objetividade e linguagem didática.

            CONCEITOS FINANCEIROS:
            - Receita é o valor obtido com vendas ou serviços.
            - Custos são gastos relacionados à produção ou aquisição dos produtos.
            - Despesas são gastos relacionados à administração e à operação.
            - Lucro é a diferença entre receitas e os custos e despesas considerados.
            - Não confunda receita com custo de produção.
            - Receita de venda é o valor obtido pela venda.
            - Custo de produção é o gasto para fabricar ou adquirir o produto.
            - Não confunda lucro bruto com lucro líquido.
            - Margem de lucro é um indicador percentual, diferente do valor do lucro.
            - Markup é um índice utilizado para formar preços.

            EXEMPLOS:
            - Identifique os exemplos numéricos como hipotéticos.
            - Use reais (R$) nos exemplos brasileiros.
            - Nunca apresente valores inventados como dados reais do DermaFer360.

            DADOS OFICIAIS:
            - Quando receber um relatório oficial, utilize os valores fornecidos.
            - Não invente nem altere receitas, despesas, datas ou indicadores.
            - Se não houver dados oficiais, responda normalmente a perguntas gerais.
            - Não afirme ter consultado o banco de dados quando isso não ocorreu.

            OBJETIVO:
            Ajudar o usuário a compreender conceitos financeiros e apoiar
            decisões responsáveis com informações claras e confiáveis.
            """;
                        

            var mensagemUsuario = pergunta;

            if (!string.IsNullOrWhiteSpace(contextoFinanceiro))
            {
                mensagemUsuario +=
                    "\n\nDados financeiros oficiais do DermaFer360:\n"
                    + contextoFinanceiro
                    + "\n\nAnalise os dados fornecidos sem inventar informações.";
            }

            var requisicao = new
            {
                model = "phi3:mini",
                stream = false,
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

            var resposta = await _httpClient.PostAsJsonAsync(
                "api/chat",
                requisicao
            );

            resposta.EnsureSuccessStatusCode();

            var resultado = await resposta.Content.ReadFromJsonAsync<OllamaResposta>();

            return resultado?.Message?.Content ?? "Não foi possível obter uma resposta da DermaFer IA.";
        }

        private class OllamaResposta
        {
            public OllamaMensagem? Message { get; set; }
        }

        private class OllamaMensagem
        {
            public string? Role { get; set; }

            public string? Content { get; set; }
        }
    }
}
