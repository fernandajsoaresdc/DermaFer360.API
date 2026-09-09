using DermaFer360.Dominio.Entidades;

public interface IDespesaRepositorio
{
    Task<int> CriarAsync(Despesa despesa);
    Task AtualizarAsync(Despesa despesa);
    Task<Despesa> ObterAsync(int despesaId);
    Task DeleteAsync(Despesa despesa);
    Task<IEnumerable<Despesa>> ListarAsync();
    Task<IEnumerable<Despesa>> ListarPorPeriodoAsync(DateTime dataInicial, DateTime dataFinal);
}

