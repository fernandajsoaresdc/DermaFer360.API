using DermaFer360.Dominio.Entidades;
using System.Net.Mail;
using DermaFer360.Aplicacao.Interfaces;
using System.Text.RegularExpressions;
using DermaFer360.Aplicacao.DTOs;



namespace DermaFer360.Aplicacao
{

    public class UsuarioAplicacao : IUsuarioAplicacao
    {
        readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<int> CriarAsync(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("Usuário não pode ser vazio.");

            var usuarioDominio = new Usuario()
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Cargo = usuario.Cargo,
                Ativo = true
            };

            InformacoesUsuario(usuarioDominio);

            if (string.IsNullOrEmpty(usuario.Senha))
                throw new Exception("A senha não pode ser vazia.");

            return await _usuarioRepositorio.CriarAsync(usuarioDominio);
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterAsync(usuario.Id);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            usuarioDominio.Nome = usuario.Nome;
            usuarioDominio.Email = usuario.Email;
            usuarioDominio.Cargo = usuario.Cargo;

            InformacoesUsuario(usuarioDominio);

            await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
        }

        public async Task AlterarSenhaAsync(Usuario usuario)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterAsync(usuario.Id);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            if (usuarioDominio.Senha != usuario.SenhaAntiga)
                throw new Exception("Senha antiga inválida.");

            usuarioDominio.Senha = usuario.Senha;

            await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
        }

        public async Task<Usuario> ObterAsync(int usuarioId)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterAsync(usuarioId);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            return new Usuario()
            {
                Id = usuarioDominio.Id,
                Nome = usuarioDominio.Nome,
                Email = usuarioDominio.Email,
                Cargo = usuarioDominio.Cargo,
                Ativo = usuarioDominio.Ativo
            };
        }

        public async Task<Usuario> ObterPorEmailAsync(string email)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorEmailAsync(email);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            return usuarioDominio;
        }

        public async Task DeletarAsync(int usuarioId)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterAsync(usuarioId);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            await _usuarioRepositorio.DeleteAsync(usuarioDominio);
        }

        public async Task RestaurarAsync(int usuarioId)
        {
            var usuario = await _usuarioRepositorio.ObterIncluindoInativosAsync(usuarioId);

            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            usuario.Restaurar();

            await _usuarioRepositorio.AtualizarAsync(usuario);
        }

        public async Task<IEnumerable<Usuario>> ListarAsync()
        {
            var usuario = await _usuarioRepositorio.ListarAsync();
            return usuario.Select(u => new Usuario()
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Cargo = u.Cargo,
                Ativo = u.Ativo
            }).ToList();
        }

        public IEnumerable<CargoUsuario> ListarCargos()
        {
               return new List<CargoUsuario>
            {
                new() { Id = 1, Nome = "Administrador" },
                new() { Id = 2, Nome = "Financeiro" },
                new() { Id = 3, Nome = "Estoque" }
            };
         
        }



        #region Util
        private static void InformacoesUsuario(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Nome))
                throw new Exception("Nome não pode ser vazio.");

            if (string.IsNullOrEmpty(usuario.Email))
                throw new Exception("Email não pode ser vazio.");

            if (!EmailValidar(usuario.Email))
                throw new Exception("O e-mail deve ser válido.");


        }

        private static bool EmailValidar(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return false;

                var enderecoEletronico = new MailAddress(email);

                return enderecoEletronico.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}