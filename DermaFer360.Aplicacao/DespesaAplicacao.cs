using DermaFer360.Dominio.Entidades;
using DermaFer360.Aplicacao.Interfaces;

namespace DermaFer360.Aplicacao
{
    public class DespesaAplicacao : IDespesaAplicacao
    {
        private readonly IDespesaRepositorio _despesaRepositorio;

        public DespesaAplicacao(IDespesaRepositorio despesaRepositorio)
        {
            _despesaRepositorio = despesaRepositorio;
        }

        public async Task<int> CriarAsync(Despesa despesa)
        {

            if (despesa == null)
                throw new Exception("Despesa não pode ser vazia.");

            var despesaDominio = new Despesa()
            {
                Descricao = despesa.Descricao,
                Valor = despesa.Valor,
                DataDespesa = despesa.DataDespesa,
                UsuarioId = despesa.UsuarioId,
                Ativo = true

            };

            InformacoesDespesa(despesaDominio);

            return await _despesaRepositorio.CriarAsync(despesaDominio);
        }

        public async Task AtualizarAsync(Despesa despesa)
        {
            var despesaDominio = await _despesaRepositorio.ObterAsync(despesa.Id);

            if (despesaDominio == null)
                throw new Exception("Despesa não encontrada.");


            despesaDominio.Descricao = despesa.Descricao;
            despesaDominio.UsuarioId = despesa.UsuarioId;
            despesaDominio.Valor = despesa.Valor;
            despesaDominio.DataDespesa = despesa.DataDespesa;

            InformacoesDespesa(despesaDominio);

            await _despesaRepositorio.AtualizarAsync(despesaDominio);

        }



        public async Task<Despesa> ObterAsync(int despesaId)
        {
            var despesaDominio = await _despesaRepositorio.ObterAsync(despesaId);

            if (despesaDominio == null)
                throw new Exception("Despesa não encontrada.");

            return new Despesa()
            {
                Id = despesaDominio.Id,
                Descricao = despesaDominio.Descricao,
                Valor = despesaDominio.Valor,
                DataDespesa = despesaDominio.DataDespesa,
                UsuarioId = despesaDominio.UsuarioId,
                Ativo = despesaDominio.Ativo
            };
        }


        public async Task DeletarAsync(int despesaId)
        {
            var despesaDominio = await _despesaRepositorio.ObterAsync(despesaId);

            if (despesaDominio == null)
                throw new Exception("Despesa não encontrada.");

            await _despesaRepositorio.DeleteAsync(despesaDominio);
        }


        public async Task<IEnumerable<Despesa>> ListarAsync()
        {
            var despesas = await _despesaRepositorio.ListarAsync();

            return despesas.Select(d => new Despesa()
            {
                Id = d.Id,
                Descricao = d.Descricao,
                Valor = d.Valor,
                DataDespesa = d.DataDespesa,
                UsuarioId = d.UsuarioId,
                Ativo = d.Ativo
            }).ToList();
        }

        public async Task<IEnumerable<Despesa>> ListarPorPeriodoAsync(DateTime dataInicial, DateTime dataFinal)
        {
            return await _despesaRepositorio.ListarPorPeriodoAsync(dataInicial, dataFinal);
        }



        #region Util
        private static void InformacoesDespesa(Despesa despesa)
        {
            if (string.IsNullOrEmpty(despesa.Descricao))
                throw new Exception("Descrição não pode ser vazio.");

            if (despesa.UsuarioId <= 0)
                throw new Exception("Usuário é obrigatório.");

            if (despesa.Valor <= 0)
                throw new Exception("O valor da despesa deve ser maior que 0.");


            if (despesa.DataDespesa.Date < DateTime.Now.Date)
                throw new Exception("A data da despesa não pode ser retroativa.");
        }
        #endregion
    }
}