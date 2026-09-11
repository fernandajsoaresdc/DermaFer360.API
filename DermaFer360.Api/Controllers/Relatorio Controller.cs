using Microsoft.AspNetCore.Mvc;
using DermaFer360.Dominio.Entidades;
using DermaFer360.Api.Models.Resposta;
using DermaFer360.Api.Models.Requisicao;
using DermaFer360.Aplicacao.Interfaces;
using DermaFer360.Aplicacao.DTO; 

namespace DermaFer360.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly IRelatorioAplicacao _relatorioAplicacao;

        public RelatorioController(IRelatorioAplicacao relatorioAplicacao)
        {
            _relatorioAplicacao = relatorioAplicacao;
        }

        [HttpPost]
        [Route("RelatorioFinanceiro")]

        public async Task<IActionResult> GerarRelatorioAsync([FromBody] RelatorioFinanceiroRequisicao relatorio)
        {
            try
            {

                var relatorioGerado = await _relatorioAplicacao.GerarRelatorioAsync(relatorio.DataInicial, relatorio.DataFinal);
                return Ok(new { relatorioGerado, mensagem = "Relatório gerado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
