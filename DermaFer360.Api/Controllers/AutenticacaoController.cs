using Microsoft.AspNetCore.Mvc;
using DermaFer360.Aplicacao.Interfaces;
using DermaFer360.Aplicacao.DTO;


namespace DermaFer360.Api
{

[ApiController]
[Route("[controller]")]

public class AutenticacaoController : ControllerBase
    {
       private readonly IAutenticacaoAplicacao _autenticacaoAplicacao;

        public AutenticacaoController(IAutenticacaoAplicacao autenticacaoAplicacao)
        {
            _autenticacaoAplicacao = autenticacaoAplicacao;
        }

        [HttpPost]
        [Route ("Login")]

        public async Task<ActionResult> LoginAsync ([FromBody] LoginAutenticacao login)
        {
            try
            {
                var autenticacao = await _autenticacaoAplicacao.LoginAsync (login);

                return Ok(autenticacao);
            }
            catch(Exception)
            {
                return BadRequest("E-mail e senha inválidos.");
            }
        }

    }

}
