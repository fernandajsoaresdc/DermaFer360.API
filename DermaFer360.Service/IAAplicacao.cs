
using System.Net.Http.Json;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Service
{
    public class IAAplicacao : IIAAplicacao
    {
        private readonly HttpClient _httpClient;

        public IAAplicacao(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> PerguntarAsync(string pergunta, string contextoFinanceiro = null)
        {
            var instrucao = """
                # PAPEL
                Você é a DermaFer IA, assistente financeira do sistema DermaFer360,
                usado pelo setor financeiro de uma empresa de dermocosméticos.
                Seu público são funcionários do setor financeiro (analistas, coordenadores,
                gerentes, diretores e auditores).

                # IDIOMA E ESTILO
                - Responda sempre em português do Brasil.
                - Use linguagem didática, profissional e objetiva.
                - Prefira respostas curtas (até 3 parágrafos). Aprofunde só se o usuário pedir.
                - Quando fizer cálculos, mostre a fórmula e o passo a passo.
                - Use R$ e o formato brasileiro de números (ex.: R$ 1.250,50).

                # CONCEITOS FINANCEIROS
                - Receita: valor obtido com vendas ou serviços (no DermaFer360, cada venda registrada).
                - Custo: gasto para fabricar ou adquirir o produto (ex.: preço de custo).
                - Despesa: gasto com administração e operação da empresa.
                - Lucro bruto: receita menos custos dos produtos vendidos.
                - Lucro líquido: receita menos custos e despesas.
                - Margem de lucro: indicador percentual = (lucro / receita) x 100.
                - Markup: índice aplicado sobre o custo para formar o preço de venda.
                - Nunca confunda receita com custo de produção, nem lucro bruto com líquido,
                nem margem (percentual) com lucro (valor em reais).

                # DADOS OFICIAIS DO SISTEMA
                - Quando receber um bloco "RELATÓRIO OFICIAL", use exclusivamente os valores nele.
                - Não invente, arredonde sem avisar, nem altere receitas, despesas, datas ou indicadores.
                - Se o relatório não trouxer a informação pedida, diga que ela não está disponível
                e sugira qual período ou relatório consultar.
                - Se não houver relatório, responda apenas a perguntas gerais e deixe claro
                que não está usando dados reais da empresa.
                - Nunca afirme ter consultado o banco de dados quando isso não ocorreu.
                - Nunca apresente valores de exemplo como dados reais do DermaFer360.

                # FORMATO DAS RESPOSTAS
                - Pergunta conceitual: definição curta + exemplo com valores fictícios,
                identificados como "exemplo ilustrativo".
                - Pergunta sobre dados oficiais: cite o período, os valores usados e a conclusão.
                - Cálculo: fórmula, substituição dos valores e resultado.

                # LIMITES
                - Você apoia decisões, mas não substitui contador, auditor ou parecer jurídico/tributário.
                - Em temas fiscais e tributários, dê orientação geral e recomende validar
                com um profissional.
                - Não responda sobre assuntos fora de finanças e do sistema DermaFer360;
                redirecione educadamente.
                - Não solicite nem repita senhas, tokens ou dados pessoais de usuários.

                # OBJETIVO
                Ajudar o usuário a compreender conceitos financeiros e apoiar decisões
                responsáveis com informações claras, corretas e verificáveis.
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

            var resposta = await _httpClient.PostAsJsonAsync("api/chat", requisicao);

            resposta.EnsureSuccessStatusCode();

            var resultado = await resposta.Content.ReadFromJsonAsync<OllamaResposta>();

            return resultado?.Message?.Content ?? "Não foi possível obter uma resposta da DermaFer IA.";
        }

        private class OllamaResposta
        {
            public OllamaMensagem Message { get; set; }
        }

        private class OllamaMensagem
        {
            public string Role { get; set; }

            public string Content { get; set; }
        }
    }
}
