using DermaFer360.Dominio.Entidades;

namespace DermaFer360.Aplicacao.Interfaces
{
    public interface IDespesaAplicacao
    {
        Task<int> CriarAsync(Despesa despesa);
        Task AtualizarAsync(Despesa despesa);
        Task<Despesa> ObterAsync(int despesaId);
        Task DeletarAsync(int despesaId);
        Task<IEnumerable<Despesa>> ListarAsync();
         Task<IEnumerable<Despesa>> ListarPorPeriodoAsync(DateTime dataInicial, DateTime dataFinal);

    }
}
