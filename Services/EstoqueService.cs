using DesafioTarget.Models;
namespace DesafioTarget.Services;

public class EstoqueService
{
    private int _proximoCodigoMovimentacao;

    public EstoqueService(IEnumerable<MovimentacaoEstoque> movimentacoes)
    {
        _proximoCodigoMovimentacao = movimentacoes
            .Select(m => m.CodigoMovimentacao)
            .DefaultIfEmpty(0)
            .Max() + 1;
    }

    public Produto? BuscarProduto(
        List<Produto> produtos,
        int codigoProduto)
    {
        return produtos.FirstOrDefault(
            p => p.CodigoProduto == codigoProduto);
    }

    public MovimentacaoEstoque RegistrarEntrada(
        Produto produto,
        int quantidade)
    {
        ValidarQuantidade(quantidade);

        produto.Estoque += quantidade;

        var movimentacao = CriarMovimentacao(
            produto,
            "ENTRADA",
            quantidade);

        return movimentacao;
    }

    public MovimentacaoEstoque RegistrarSaida(
        Produto produto,
        int quantidade)
    {
        ValidarQuantidade(quantidade);

        if (quantidade > produto.Estoque)
        {
            throw new InvalidOperationException(
                "Estoque insuficiente para realizar a saída.");
        }

        produto.Estoque -= quantidade;

        var movimentacao = CriarMovimentacao(
            produto,
            "SAÍDA",
            quantidade);

        return movimentacao;
    }

    private MovimentacaoEstoque CriarMovimentacao(
        Produto produto,
        string tipo,
        int quantidade)
    {
        return new MovimentacaoEstoque
        {
            CodigoMovimentacao = _proximoCodigoMovimentacao++,
            CodigoProduto = produto.CodigoProduto,
            Tipo = tipo,
            Quantidade = quantidade,
            Data = DateTime.Now
        };
    }

    private static void ValidarQuantidade(int quantidade)
    {
        if (quantidade <= 0)
        {
            throw new ArgumentException(
                "A quantidade deve ser maior que zero.");
        }
    }
}