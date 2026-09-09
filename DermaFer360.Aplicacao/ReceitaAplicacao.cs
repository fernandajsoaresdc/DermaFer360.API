using DermaFer360.Dominio.Entidades;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Aplicacao
{
    public class ReceitaAplicacao : IReceitaAplicacao
    {
        private readonly IReceitaRepositorio _receitaRepositorio;

        public ReceitaAplicacao(IReceitaRepositorio receitaRepositorio)
        {
            _receitaRepositorio = receitaRepositorio;
        }

        public async Task<int> CriarAsync(Receita receita)
        {

            if (receita == null)
                throw new Exception("Receita não pode ser vazia.");

            InformacoesReceita(receita);

            return await _receitaRepositorio.CriarAsync(receita);
        }

        public async Task AtualizarAsync(Receita receita)
        {
            var receitaDominio = await _receitaRepositorio.ObterAsync(receita.Id);

            if (receitaDominio == null)
                throw new Exception("Receita não encontrada.");

            InformacoesReceita(receita);

            receitaDominio.ProdutoId = receita.ProdutoId;
            receitaDominio.UsuarioId = receita.UsuarioId;
            receitaDominio.PrecoUnitario = receita.PrecoUnitario;
            receitaDominio.DataReceita = receita.DataReceita;

            await _receitaRepositorio.AtualizarAsync(receitaDominio);

        }

        public async Task<Receita> ObterAsync(int receitaId)
        {
            var receitaDominio = await _receitaRepositorio.ObterAsync(receitaId);

            if (receitaDominio == null)
                throw new Exception("Receita não encontrada.");

            return receitaDominio;
        }

        public async Task DeletarAsync(int receitaId)
        {
            var receitaDominio = await _receitaRepositorio.ObterAsync(receitaId);

            if (receitaDominio == null)
                throw new Exception("Receita não encontrada.");

            await _receitaRepositorio.DeleteAsync(receitaDominio);
        }

        public async Task<IEnumerable<Receita>> ListarAsync()
        {
            return await _receitaRepositorio.ListarAsync();
        }

        public async Task<IEnumerable<Receita>> ListarPorPeriodoAsync(DateTime dataInicial, DateTime dataFinal)
        {
            return await _receitaRepositorio.ListarPorPeriodoAsync(dataInicial, dataFinal);
        }



        #region Util
        private static void InformacoesReceita(Receita receita)
        {
            if (receita.ProdutoId <= 0)
                throw new Exception("Produto é obrigatório.");

            if (receita.UsuarioId <= 0)
                throw new Exception("Usuário é obrigatório.");

            if (receita.PrecoUnitario <= 0)
                throw new Exception("O valor da receita deve ser maior que zero.");

            if (receita.DataReceita > DateTime.Now)
                throw new Exception("A data da receita não pode ser futura.");
        }
        #endregion
    }
}