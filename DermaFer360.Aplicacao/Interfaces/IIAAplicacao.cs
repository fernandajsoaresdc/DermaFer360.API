
namespace DermaFer360.Aplicacao.Interfaces
{
    public interface IIAAplicacao
    {
        Task<string> PerguntarAsync( string pergunta, string? contextoFinanceiro = null );
    }
}
