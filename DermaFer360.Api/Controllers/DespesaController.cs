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

        public async Task<IActionResult> CriarAsync([FromBody] DespesaCriar despesa)
        {
            try
            {
                var despesaDominio = new Despesa()
                {
                    Descricao = despesa.Descricao,
                    Valor = despesa.Valor,
                    DataDespesa = despesa.DataDespesa,
                    UsuarioId = despesa.UsuarioId,
                    Ativo = true

                };

                var despesaId = await _despesaAplicacao.CriarAsync(despesaDominio);
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
                var despesaDominio = await _despesaAplicacao.ObterAsync(despesaId);
                var despesaResposta = new DespesaResposta()
                {
                    Id = despesaDominio.Id,
                    Descricao = despesaDominio.Descricao,
                    Valor = despesaDominio.Valor,
                    DataDespesa = despesaDominio.DataDespesa,
                    UsuarioId = despesaDominio.UsuarioId,
                    Ativo = true
                };

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Atualizar")]

        public async Task<IActionResult> AtualizarAsync([FromBody] DespesaAtualizar despesa)
        {
            try
            {
                var despesaDominio = new Despesa()
                {
                    Id = despesa.Id,
                    Descricao = despesa.Descricao,
                    Valor = despesa.Valor,
                    DataDespesa = despesa.DataDespesa,
                    UsuarioId = despesa.UsuarioId,
                    Ativo = true

                };

                await _despesaAplicacao.AtualizarAsync(despesaDominio);
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

                var despesas = despesaDominio.Select(d => new Despesa()
                {
                    Id = d.Id,
                    Descricao = d.Descricao,
                    Valor = d.Valor,
                    DataDespesa = d.DataDespesa,
                    UsuarioId = d.UsuarioId,
                    Ativo = true
                }).ToList();

                return Ok(despesas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}