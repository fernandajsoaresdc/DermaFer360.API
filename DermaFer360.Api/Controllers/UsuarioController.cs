using Microsoft.AspNetCore.Mvc;
using DermaFer360.Dominio.Entidades;
using DermaFer360.Api.Models.Resposta;
using DermaFer360.Api.Models.Requisicao;
using DermaFer360.Aplicacao.Interfaces;


namespace DermaFer360.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioAplicacao _usuarioAplicacao;

        public UsuarioController(IUsuarioAplicacao usuarioAplicacao)
        {
            _usuarioAplicacao = usuarioAplicacao;
        }

        [HttpPost]
        [Route("Criar")]

        public async Task<IActionResult> CriarAsync([FromBody] UsuarioCriar usuario)
        {
            try
            {
                var usuarioDominio = new Usuario()
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Senha = usuario.Senha,
                    Cargo = usuario.Cargo,
                    Ativo = true

                };

                var usuarioId = await _usuarioAplicacao.CriarAsync(usuarioDominio);
                return Ok(new { usuarioId, mensagem = "Usuario cadastrado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("Obter/{usuarioId}")]

        public async Task<IActionResult> ObterAsync([FromRoute] int usuarioId)
        {
            try
            {
                var usuarioDominio = await _usuarioAplicacao.ObterAsync(usuarioId);
                var usuarioResposta = new UsuarioResposta()
                {
                    Id = usuarioDominio.Id,
                    Nome = usuarioDominio.Nome,
                    Email = usuarioDominio.Email,
                    Cargo = usuarioDominio.Cargo,
                    Ativo = usuarioDominio.Ativo
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

        public async Task<IActionResult> AtualizarAsync([FromBody] UsuarioAtualizar usuario)
        {
            try
            {
                var usuarioDominio = new Usuario()
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Cargo = usuario.Cargo,
                    Ativo = usuario.Ativo
                };

                await _usuarioAplicacao.AtualizarAsync(usuarioDominio);
                return Ok("Usuário atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("AlterarSenha")]

        public async Task<IActionResult> AlterarSenhaAsync([FromBody] UsuarioAlterarSenha usuario)
        {
            try
            {
                var usuarioDominio = new Usuario()
                {
                    Id = usuario.Id,
                    Senha = usuario.Senha
                };

                await _usuarioAplicacao.AlterarSenhaAsync(usuarioDominio, usuario.SenhaAntiga);
                return Ok(new { mensagem = "Senha alterada com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Deletar/{usuarioId}")]

        public async Task<IActionResult> DeletarAsync([FromRoute] int usuarioId)
        {
            try
            {
                await _usuarioAplicacao.DeletarAsync(usuarioId);
                return Ok(new { mensagem = "Usuário removido com sucesso." });
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
                var usuarioDominio = await _usuarioAplicacao.ListarAsync();

                var usuarios = usuarioDominio.Select(u => new Usuario()
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Cargo = u.Cargo,
                    Ativo = u.Ativo
                }).ToList();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ListarCargos")]

        public IActionResult ListarCargos()
        {
            var cargos = new List<object>
            {
                new { Id = 1, Nome = "Administrador" },
                new { Id = 2, Nome = "Financeiro" },
                new { Id = 3, Nome = "Estoque" }
            };

            return Ok(cargos);
        }
    }
}