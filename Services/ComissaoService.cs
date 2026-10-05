using DesafioTarget.Models;
namespace DesafioTarget.Services;

public class ComissaoService
{
    public decimal CalcularComissao(Venda venda)
    {
        if (venda.Valor < 100)
        {
            return 0;
        }

        if (venda.Valor < 500)
        {
            return venda.Valor * 0.01m;
        }

        return venda.Valor * 0.05m;
    }

    public Dictionary<string, decimal> CalcularComissoes(
        List<Venda> vendas)
    {
        return vendas
            .GroupBy(v => v.Vendedor)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Sum(CalcularComissao)
            );
    }
}