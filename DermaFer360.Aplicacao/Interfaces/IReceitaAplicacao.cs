using DermaFer360.Dominio.Entidades;

namespace DermaFer360.Aplicacao.Interfaces
{
    public interface IReceitaAplicacao
    {
        Task<int> CriarAsync(Receita receita);
        Task AtualizarAsync(Receita receita);
        Task<Receita> ObterAsync(int receitaId);
        Task DeletarAsync(int receitaId);
        Task<IEnumerable<Receita>> ListarAsync(); 
        Task<IEnumerable<Receita>> ListarPorPeriodoAsync(DateTime dataInicial, DateTime dataFinal);  
    }
}