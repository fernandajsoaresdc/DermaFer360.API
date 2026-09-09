
using DermaFer360.Dominio.Entidades;
using DermaFer360.Aplicacao.Interfaces;


namespace DermaFer360.Aplicacao
{
    public class ProdutoAplicacao : IProdutoAplicacao
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoAplicacao(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<int> CriarAsync(Produto produto)
        {

            if (produto == null)
                throw new Exception("Produto não pode ser vazio.");

            InformacoesProduto(produto);

            return await _produtoRepositorio.CriarAsync(produto);
        }

        public async Task AtualizarAsync(Produto produto)
        {
            var produtoDominio = await _produtoRepositorio.ObterAsync(produto.Id);

            if(produtoDominio == null)
            throw new Exception("Produto não encontrado.");

            InformacoesProduto(produto);

            produtoDominio.Nome = produto.Nome;
            produtoDominio.PrecoCusto = produto.PrecoCusto;
            produtoDominio.PrecoVenda = produto.PrecoVenda;

            await _produtoRepositorio.AtualizarAsync(produtoDominio);

        }

        public async Task<Produto> ObterAsync(int produtoId)
        {
            var produtoDominio = await _produtoRepositorio.ObterAsync(produtoId);

            if(produtoDominio == null)
            throw new Exception("Produto não encontrado.");

            return produtoDominio;
        }

        public async Task DeletarAsync(int produtoId)
        {
            var produtoDominio = await _produtoRepositorio.ObterAsync(produtoId);

            if(produtoDominio == null)
            throw new Exception("Produto não encontrado.");

            await _produtoRepositorio.DeleteAsync(produtoDominio);
        }

        public async Task<IEnumerable<Produto>> ListarAsync()
        {
            return await _produtoRepositorio.ListarAsync();
        }



        #region Util
        private static void InformacoesProduto(Produto produto)
        {
            if (string.IsNullOrEmpty(produto.Nome))
                throw new Exception("Nome do produto não pode ser vazio.");

            if (produto.PrecoCusto <= 0)
                throw new Exception("O custo do produto deve ser maior que zero.");

            if (produto.PrecoVenda <= 0)
                throw new Exception("O preço de venda do produto deve ser maior que zero.");

            if (produto.PrecoVenda <= produto.PrecoCusto)
                throw new Exception("O preço de venda deve ser maior que o preco de custo.");
        }
        #endregion
    }
}