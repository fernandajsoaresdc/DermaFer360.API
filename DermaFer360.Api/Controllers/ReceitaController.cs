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

        public async Task<IActionResult> CriarAsync([FromBody] ReceitaCriar receita)
        {
            try
            {
                var receitaDominio = new Receita()
                {
                    ProdutoId = receita.ProdutoId,
                    Quantidade = receita.Quantidade,
                    PrecoUnitario = receita.PrecoUnitario,
                    DataReceita = receita.DataReceita,
                    UsuarioId = receita.UsuarioId,
                    Ativo = true

                };

                var receitaId = await _receitaAplicacao.CriarAsync(receitaDominio);
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
                var receitaDominio = await _receitaAplicacao.ObterAsync(receitaId);
                var receitaResposta = new ReceitaResposta()
                {
                    Id = receitaDominio.Id,
                    ProdutoId = receitaDominio.ProdutoId,
                    Quantidade = receitaDominio.Quantidade,
                    PrecoUnitario = receitaDominio.PrecoUnitario,
                    DataReceita = receitaDominio.DataReceita,
                    UsuarioId = receitaDominio.UsuarioId,
                    Ativo = receitaDominio.Ativo
                };

                return Ok(receitaResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Atualizar")]

        public async Task<IActionResult> AtualizarAsync([FromBody] ReceitaAtualizar receita)
        {
            try
            {
                var receitaDominio = new Receita()
                {
                    Id = receita.Id,
                    ProdutoId = receita.ProdutoId,
                    Quantidade = receita.Quantidade,
                    PrecoUnitario = receita.PrecoUnitario,
                    DataReceita = receita.DataReceita,
                    UsuarioId = receita.UsuarioId,
                    Ativo = receita.Ativo
                };

                await _receitaAplicacao.AtualizarAsync(receitaDominio);
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

                var receitas = receitaDominio.Select(r => new ReceitaResposta()
                {
                    Id = r.Id,
                    ProdutoId = r.ProdutoId,
                    Quantidade = r.Quantidade,
                    PrecoUnitario = r.PrecoUnitario,
                    DataReceita = r.DataReceita,
                    UsuarioId = r.UsuarioId,
                    Ativo = r.Ativo
                }).ToList();

                return Ok(receitas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}