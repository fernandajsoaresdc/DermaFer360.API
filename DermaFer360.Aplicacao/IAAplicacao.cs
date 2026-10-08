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

        public async Task<string> PerguntarAsync(string pergunta)
        {
            var requisicao = new
            {
                model = "phi3:mini",
                stream = false,
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = """
                        Você é a DermaFer IA, assistente especializada em
                        governança financeira de empresas de dermocosméticos.

                        Responda em português, de forma clara, objetiva e didática.

                        Você pode explicar conceitos como:
                        custos, despesas, receitas, lucro, margem de lucro,
                        markup, fluxo de caixa, precificação e indicadores financeiros.

                        Não invente dados financeiros da empresa.

                        Quando a pergunta depender de dados reais do DermaFer360,
                        informe que é necessário consultar os dados do sistema.
                        """
                    },
                    new
                    {
                        role = "user",
                        content = pergunta
                    }
                }
            };

            var resposta = await _httpClient.PostAsJsonAsync(
                "api/chat",
                requisicao
            );

            resposta.EnsureSuccessStatusCode();

            var resultado =
                await resposta.Content.ReadFromJsonAsync<OllamaResposta>();

            return resultado?.Message?.Content
                   ?? "Não foi possível obter uma resposta da IA.";
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