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

        public async Task<IActionResult> CriarAsync([FromBody] ProdutoCriar produto)
        {
            try
            {
                var produtoDominio = new Produto()
                {
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    PrecoCusto = produto.PrecoCusto,
                    PrecoVenda = produto.PrecoVenda,
                    UsuarioId = produto.UsuarioId,
                    Estoque = produto.Estoque,
                    Ativo = true

                };

                var produtoId = await _produtoAplicacao.CriarAsync(produtoDominio);
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
                var produtoDominio = await _produtoAplicacao.ObterAsync(produtoId);
                var produtoResposta = new ProdutoResposta()
                {
                    Id = produtoDominio.Id,
                    Nome = produtoDominio.Nome,
                    Descricao = produtoDominio.Descricao,
                    PrecoCusto = produtoDominio.PrecoCusto,
                    PrecoVenda = produtoDominio.PrecoVenda,
                    Estoque = produtoDominio.Estoque,
                    Ativo = produtoDominio.Ativo
                };

                return Ok(produtoResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Atualizar")]

        public async Task<IActionResult> AtualizarAsync([FromBody] ProdutoAtualizar produto)
        {
            try
            {
                var produtoDominio = new Produto()
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    PrecoCusto = produto.PrecoCusto,
                    PrecoVenda = produto.PrecoVenda,
                    Ativo = produto.Ativo
                };

                await _produtoAplicacao.AtualizarAsync(produtoDominio);
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

                var produtos = produtoDominio.Select(p => new ProdutoResposta()
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    PrecoCusto = p.PrecoCusto,
                    PrecoVenda = p.PrecoVenda,
                    Estoque = p.Estoque,
                    Ativo = p.Ativo
                }).ToList();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
