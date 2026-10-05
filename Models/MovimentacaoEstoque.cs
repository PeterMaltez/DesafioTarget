namespace DesafioTarget.Models;

public class MovimentacaoEstoque
{
    public int CodigoMovimentacao { get; set; }
    public int CodigoProduto { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public DateTime Data { get; set; }
}