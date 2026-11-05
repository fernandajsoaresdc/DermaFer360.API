using Microsoft.AspNetCore.Mvc;
using DermaFer360.Dominio.Entidades;
using DermaFer360.Api.Models.Resposta;
using DermaFer360.Api.Models.Requisicao;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ReceitaController : ControllerBase
    {
        private readonly IReceitaAplicacao _receitaAplicacao;

        public ReceitaController(IReceitaAplicacao receitaAplicacao)
        {
            _receitaAplicacao = receitaAplicacao;
        }

        [HttpPost]
        [Route("Criar")]

        public async Task<IActionResult> CriarAsync([FromBody] Receita receita)
        {
            try
            {

                var receitaId = await _receitaAplicacao.CriarAsync(receita);
                return Ok(new {receitaId, mensagem = "Receita cadastrada com sucesso."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("Obter/{receitaId}")]

        public async Task<IActionResult> ObterAsync([FromRoute] int receitaId)
        {
            try
            {
                var receitaResposta = await _receitaAplicacao.ObterAsync(receitaId);
                return Ok(receitaResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Atualizar")]

        public async Task<IActionResult> AtualizarAsync([FromBody] Receita receita)
        {
            try
            {
                await _receitaAplicacao.AtualizarAsync(receita);

                return Ok("Receita atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("Deletar/{receitaId}")]

        public async Task<IActionResult> DeletarAsync([FromRoute] int receitaId)
        {
            try
            {
                await _receitaAplicacao.DeletarAsync(receitaId);
                return Ok(new {mensagem = "Receita removida com sucesso."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Listar")]

        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                var receitaDominio = await _receitaAplicacao.ListarAsync();

                return Ok(receitaDominio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}