using DermaFer360.Dominio.Entidades;
using System.Net.Mail;
using DermaFer360.Aplicacao.Interfaces;
using System.Text.RegularExpressions;
using System.Net.Mail;



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

            InformacoesUsuario(usuario);

            if (string.IsNullOrEmpty(usuario.Senha))
                throw new Exception("A senha não pode ser vazia.");

            return await _usuarioRepositorio.CriarAsync(usuario);
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterAsync(usuario.Id);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            InformacoesUsuario(usuario);

            usuarioDominio.Nome = usuario.Nome;
            usuarioDominio.Email = usuario.Email;
            usuarioDominio.Cargo = usuario.Cargo;

            await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
        }

        public async Task AlterarSenhaAsync(Usuario usuario, string senhaAntiga)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterAsync(usuario.Id);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            if (usuarioDominio.Senha != senhaAntiga)
                throw new Exception("Senha antiga inválida.");

            usuarioDominio.Senha = usuario.Senha;

            await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
        }

        public async Task<Usuario> ObterAsync(int usuarioId)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterAsync(usuarioId);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            return usuarioDominio;
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
            return await _usuarioRepositorio.ListarAsync();
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

        //         private static bool EmailValidar(string email)
        // {
        //     try
        //     {
        //         var enderecoEletronico = new MailAddress(email);
        //         return enderecoEletronico.Address == email;
        //     }
        //     catch
        //     {
        //         return false;
        //     }
        // }
        #endregion
    }
}