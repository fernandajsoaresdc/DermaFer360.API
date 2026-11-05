using Microsoft.AspNetCore.Mvc;
using DermaFer360.Dominio.Entidades;
using DermaFer360.Api.Models.Resposta;
using DermaFer360.Api.Models.Requisicao;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DespesaController : ControllerBase
    {
        private readonly IDespesaAplicacao _despesaAplicacao;

        public DespesaController(IDespesaAplicacao despesaAplicacao)
        {
            _despesaAplicacao = despesaAplicacao;
        }

        [HttpPost]
        [Route("Criar")]

        public async Task<IActionResult> CriarAsync([FromBody] Despesa despesa)
        {
            try
            {
               
                var despesaId = await _despesaAplicacao.CriarAsync(despesa);
                return Ok(new { despesaId, mensagem = "Despesa cadastrada com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("Obter/{despesaId}")]
        public async Task<IActionResult> ObterAsync([FromRoute] int despesaId)
        {
            try
            {
                var despesaResposta = await _despesaAplicacao.ObterAsync(despesaId);
                return Ok(despesaResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Atualizar")]

        public async Task<IActionResult> AtualizarAsync([FromBody] Despesa despesa)
        {
            try
            {
                await _despesaAplicacao.AtualizarAsync(despesa);
                return Ok("Despesa atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("Deletar/{despesaId}")]

        public async Task<IActionResult> DeletarAsync([FromRoute] int despesaId)
        {
            try
            {
                await _despesaAplicacao.DeletarAsync(despesaId);
                return Ok(new { mensagem = "Despesa removida com sucesso." });
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
                var despesaDominio = await _despesaAplicacao.ListarAsync();

                return Ok(despesaDominio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}