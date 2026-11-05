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

            var receitaDominio = new Receita()
            {
                ProdutoId = receita.ProdutoId,
                Quantidade = receita.Quantidade,
                PrecoUnitario = receita.PrecoUnitario,
                DataReceita = receita.DataReceita,
                UsuarioId = receita.UsuarioId,
                Ativo = true

            };

            InformacoesReceita(receitaDominio);

            return await _receitaRepositorio.CriarAsync(receita);
        }

        public async Task AtualizarAsync(Receita receita)
        {
            var receitaDominio = await _receitaRepositorio.ObterAsync(receita.Id);

            if (receitaDominio == null)
                throw new Exception("Receita não encontrada.");

            receitaDominio.ProdutoId = receita.ProdutoId;
            receitaDominio.UsuarioId = receita.UsuarioId;
            receitaDominio.PrecoUnitario = receita.PrecoUnitario;
            receitaDominio.DataReceita = receita.DataReceita;

            InformacoesReceita(receitaDominio);

            await _receitaRepositorio.AtualizarAsync(receitaDominio);

        }

        public async Task<Receita> ObterAsync(int receitaId)
        {
            var receitaDominio = await _receitaRepositorio.ObterAsync(receitaId);

            if (receitaDominio == null)
                throw new Exception("Receita não encontrada.");

            return new Receita()
            {
                Id = receitaDominio.Id,
                ProdutoId = receitaDominio.ProdutoId,
                Quantidade = receitaDominio.Quantidade,
                PrecoUnitario = receitaDominio.PrecoUnitario,
                DataReceita = receitaDominio.DataReceita,
                UsuarioId = receitaDominio.UsuarioId,
                Ativo = receitaDominio.Ativo
            };


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
            var receita = await _receitaRepositorio.ListarAsync();
            return receita.Select(r => new Receita()
            {
                Id = r.Id,
                ProdutoId = r.ProdutoId,
                Quantidade = r.Quantidade,
                PrecoUnitario = r.PrecoUnitario,
                DataReceita = r.DataReceita,
                UsuarioId = r.UsuarioId,
                Ativo = r.Ativo
            }).ToList();
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

            if (receita.DataReceita.Date < DateTime.Now.Date)
                throw new Exception("A data da receita não pode ser retroativa.");
        }
        #endregion
    }
}