using Microsoft.AspNetCore.Mvc;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IAController : ControllerBase
    {
        private readonly IIAAplicacao _iaAplicacao;
        private readonly ILogger<IAController> _logger;

        public IAController(IIAAplicacao iaAplicacao,ILogger<IAController> logger)
        {
            _iaAplicacao = iaAplicacao;
            _logger = logger;
        }

        [HttpPost("Perguntar")]
        public async Task<IActionResult> Perguntar([FromBody] PerguntaIARequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Pergunta))
            {
                return BadRequest(new{resposta = "Digite uma pergunta."});
            }

            try
            {
                string resposta = await _iaAplicacao.PerguntarAsync(request.Pergunta.Trim());

                return Ok(new{resposta});
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Erro na comunicação com o Ollama.");

                return StatusCode(503, new{resposta = "Não foi possível conectar à inteligência artificial. Verifique se o Ollama está funcionando."});
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Tempo limite excedido na IA.");

                return StatusCode(504, new{resposta = "A IA demorou para responder. Tente novamente."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar pergunta da IA.");

                return StatusCode(500, new{resposta = "Não foi possível processar sua pergunta. Tente novamente."});
            }
        }
    }

    public class PerguntaIARequest
    {
        public string Pergunta { get; set; } = string.Empty;
    }
}