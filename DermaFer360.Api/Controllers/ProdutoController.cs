using Microsoft.AspNetCore.Mvc;
using DermaFer360.Dominio.Entidades;
using DermaFer360.Api.Models.Resposta;
using DermaFer360.Api.Models.Requisicao;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoAplicacao _produtoAplicacao;

        public ProdutoController(IProdutoAplicacao produtoAplicacao)
        {
            _produtoAplicacao = produtoAplicacao;
        }

        [HttpPost]
        [Route("Criar")]

        public async Task<IActionResult> CriarAsync([FromBody] Produto produto)
        {
            try
            {
                var produtoId = await _produtoAplicacao.CriarAsync(produto);
                return Ok(new { produtoId, mensagem = "Produto cadastrado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message, innerErro = ex.InnerException?.Message });
            }

        }

        [HttpGet]
        [Route("Obter/{produtoId}")]

        public async Task<IActionResult> ObterAsync([FromRoute] int produtoId)
        {
            try
            {
                var produtoResposta = await _produtoAplicacao.ObterAsync(produtoId);
                return Ok(produtoResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Atualizar")]

        public async Task<IActionResult> AtualizarAsync([FromBody] Produto produto)
        {
            try
            {

                await _produtoAplicacao.AtualizarAsync(produto);
                return Ok("Produto atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("Deletar/{produtoId}")]

        public async Task<IActionResult> DeletarAsync([FromRoute] int produtoId)
        {
            try
            {
                await _produtoAplicacao.DeletarAsync(produtoId);
                return Ok(new { mensagem = "Produto removido com sucesso." });
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
                var produtoDominio = await _produtoAplicacao.ListarAsync();
                return Ok(produtoDominio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
