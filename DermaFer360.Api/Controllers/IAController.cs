using DermaFer360.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DermaFer360.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class IAController : ControllerBase
    {
        private readonly IIAAplicacao _iaAplicacao;

        public IAController(IIAAplicacao iaAplicacao)
        {
            _iaAplicacao = iaAplicacao;
        }

        [HttpPost("Perguntar")]
        public async Task<IActionResult> Perguntar([FromBody] PerguntaIARequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Pergunta))
            {
                return BadRequest("A pergunta é obrigatória.");
            }

            var resposta = await _iaAplicacao.PerguntarAsync(request.Pergunta);

            return Ok(new
            {
                resposta
            });
        }
    }

    public class PerguntaIARequest
    {
        public string Pergunta { get; set; }
    }
}