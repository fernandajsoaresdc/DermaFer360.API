using DermaFer360.Dominio.Entidades;

public interface IReceitaRepositorio
{
    Task<int> CriarAsync(Receita receita);
    Task AtualizarAsync(Receita receita);
    Task<Receita> ObterAsync(int receitaId);
    Task DeleteAsync(Receita receita);
    Task<IEnumerable<Receita>> ListarAsync();
}

