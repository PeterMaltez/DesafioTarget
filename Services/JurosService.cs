using DesafioTarget.Models;
namespace DesafioTarget.Services;

public class JurosService
{
    private const decimal TaxaDiaria = 0.025m;

    public ResultadoJuros Calcular(
        decimal valor,
        DateTime dataVencimento)
    {
        if (valor < 0)
        {
            throw new ArgumentException(
                "O valor não pode ser negativo.");
        }

        DateTime hoje = DateTime.Today;

        if (dataVencimento.Date >= hoje)
        {
            return new ResultadoJuros
            {
                ValorOriginal = valor,
                DiasAtraso = 0,
                Juros = 0,
                ValorTotal = valor
            };
        }

        int diasAtraso = (hoje - dataVencimento.Date).Days;

        decimal juros = valor * TaxaDiaria * diasAtraso;

        return new ResultadoJuros
        {
            ValorOriginal = valor,
            DiasAtraso = diasAtraso,
            Juros = juros,
            ValorTotal = valor + juros
        };
    }
}

public class ResultadoJuros
{
    public decimal ValorOriginal { get; set; }
    public int DiasAtraso { get; set; }
    public decimal Juros { get; set; }
    public decimal ValorTotal { get; set; }
}