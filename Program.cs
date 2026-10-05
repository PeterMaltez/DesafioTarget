using DesafioTarget.Models;
using DesafioTarget.Services;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("========================================");
Console.WriteLine("       TARGET SISTEMAS - DESAFIO");
Console.WriteLine("========================================");

var dadosVendas = CarregarVendas();
var dadosEstoque = CarregarEstoque();
var movimentacoes = CarregarMovimentacoes();

if (dadosVendas == null || dadosEstoque == null)
{
    Console.WriteLine("Não foi possível carregar os dados iniciais.");
    return;
}

var comissaoService = new ComissaoService();
var estoqueService = new EstoqueService(movimentacoes);

bool continuar = true;

while (continuar)
{
    Console.WriteLine();
    Console.WriteLine("=== MENU ===");
    Console.WriteLine("1 - Desafio 1: Cálculo de comissões");
    Console.WriteLine("2 - Desafio 2: Movimentação de estoque");
    Console.WriteLine("3 - Desafio 3: Cálculo de juros");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opção: ");

    var opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            ExecutarDesafioComissoes(dadosVendas, comissaoService);
            break;

        case "2":
            ExecutarDesafioEstoque(estoqueService, dadosEstoque, movimentacoes);
            break;

        case "3":
            ExecutarDesafioJuros();
            break;

        case "0":
            continuar = false;
            Console.WriteLine("Programa encerrado.");
            break;

        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

static VendaData? CarregarVendas()
{
    string caminho = Path.Combine("Data", "vendas.json");

    if (!File.Exists(caminho))
    {
        Console.WriteLine($"Arquivo não encontrado: {caminho}");
        return null;
    }

    string json = File.ReadAllText(caminho);

    var dados = JsonSerializer.Deserialize<VendaData>(
        json,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

    if (dados == null)
    {
        return null;
    }

    dados.Vendas ??= new List<Venda>();
    return dados;
}

static EstoqueData? CarregarEstoque()
{
    string caminho = Path.Combine("Data", "estoque.json");

    if (!File.Exists(caminho))
    {
        Console.WriteLine($"Arquivo não encontrado: {caminho}");
        return null;
    }

    string json = File.ReadAllText(caminho);

    var dados = JsonSerializer.Deserialize<EstoqueData>(
        json,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

    if (dados == null)
    {
        return null;
    }

    dados.Estoque ??= new List<Produto>();
    return dados;
}

static List<MovimentacaoEstoque> CarregarMovimentacoes()
{
    string caminho = Path.Combine("Data", "movimentacoes.json");

    if (!File.Exists(caminho))
    {
        return new List<MovimentacaoEstoque>();
    }

    string json = File.ReadAllText(caminho);

    var dados = JsonSerializer.Deserialize<MovimentacaoData>(
        json,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

    return dados?.Movimentacoes ?? new List<MovimentacaoEstoque>();
}

static void SalvarEstoque(EstoqueData dados)
{
    var caminho = Path.Combine("Data", "estoque.json");
    var json = JsonSerializer.Serialize(dados, CriarOpcoesJson());

    File.WriteAllText(caminho, json);
}

static void SalvarMovimentacoes(List<MovimentacaoEstoque> movimentacoes)
{
    var caminho = Path.Combine("Data", "movimentacoes.json");
    var dados = new MovimentacaoData { Movimentacoes = movimentacoes };
    var json = JsonSerializer.Serialize(dados, CriarOpcoesJson());

    File.WriteAllText(caminho, json);
}

static JsonSerializerOptions CriarOpcoesJson()
{
    return new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}

static void ExecutarDesafioComissoes(VendaData dados, ComissaoService service)
{
    Console.WriteLine();
    Console.WriteLine("--- DESAFIO 1: CÁLCULO DE COMISSÕES ---");

    var comissoes = service.CalcularComissoes(dados.Vendas);

    foreach (var comissao in comissoes)
    {
        Console.WriteLine($"{comissao.Key}: {comissao.Value:C2}");
    }
}

static void ExecutarDesafioEstoque(EstoqueService service, EstoqueData dadosEstoque, List<MovimentacaoEstoque> movimentacoes)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("--- DESAFIO 2: MOVIMENTAÇÃO DE ESTOQUE ---");
        Console.WriteLine("1 - Registrar movimentação");
        Console.WriteLine("2 - Ver histórico de movimentações");
        Console.WriteLine("0 - Voltar");
        Console.Write("Escolha uma opção: ");

        var opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                RegistrarMovimentacaoEstoque(service, dadosEstoque, movimentacoes);
                break;

            case "2":
                ExibirHistoricoMovimentacoes(dadosEstoque, movimentacoes);
                break;

            case "0":
                return;

            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
}

static void RegistrarMovimentacaoEstoque(EstoqueService service, EstoqueData dadosEstoque, List<MovimentacaoEstoque> movimentacoes)
{
    if (dadosEstoque.Estoque.Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return;
    }

    for (int i = 0; i < dadosEstoque.Estoque.Count; i++)
    {
        var produto = dadosEstoque.Estoque[i];
        Console.WriteLine($"{i + 1} - {produto.CodigoProduto} - {produto.DescricaoProduto} (Estoque atual: {produto.Estoque})");
    }

    Console.WriteLine("0 - Voltar");
    Console.Write("Escolha o produto: ");
    var escolhaProduto = Console.ReadLine();

    if (escolhaProduto != null && escolhaProduto.Trim() == "0")
    {
        Console.WriteLine("Retornando ao menu de movimentação.");
        return;
    }

    if (!int.TryParse(escolhaProduto, out int indiceProduto) || indiceProduto < 1 || indiceProduto > dadosEstoque.Estoque.Count)
    {
        Console.WriteLine("Produto inválido.");
        return;
    }

    var produtoSelecionado = dadosEstoque.Estoque[indiceProduto - 1];

    Console.Write("Tipo da movimentação (E = Entrada / S = Saída): ");
    var tipo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(tipo) || (!tipo.Trim().Equals("E", StringComparison.OrdinalIgnoreCase) && !tipo.Trim().Equals("S", StringComparison.OrdinalIgnoreCase)))
    {
        Console.WriteLine("Tipo inválido. Digite apenas 'E' para entrada ou 'S' para saída.");
        return;
    }

    Console.Write("Quantidade: ");
    if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
    {
        Console.WriteLine("Quantidade inválida. A quantidade deve ser maior que zero.");
        return;
    }

    try
    {
        MovimentacaoEstoque movimentacao;

        if (tipo.Trim().Equals("E", StringComparison.OrdinalIgnoreCase))
        {
            movimentacao = service.RegistrarEntrada(produtoSelecionado, quantidade);
            Console.WriteLine("Entrada registrada com sucesso.");
        }
        else
        {
            movimentacao = service.RegistrarSaida(produtoSelecionado, quantidade);
            Console.WriteLine("Saída registrada com sucesso.");
        }

        movimentacoes.Add(movimentacao);
        SalvarEstoque(dadosEstoque);
        SalvarMovimentacoes(movimentacoes);

        Console.WriteLine($"Código da movimentação: {movimentacao.CodigoMovimentacao}");
        Console.WriteLine($"Tipo: {movimentacao.Tipo}");
        Console.WriteLine($"Quantidade movimentada: {movimentacao.Quantidade}");
        Console.WriteLine($"Estoque final do produto: {produtoSelecionado.Estoque}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

static void ExibirHistoricoMovimentacoes(EstoqueData dadosEstoque, List<MovimentacaoEstoque> movimentacoes)
{
    Console.WriteLine();
    Console.WriteLine("--- HISTÓRICO DE MOVIMENTAÇÕES ---");

    if (movimentacoes.Count == 0)
    {
        Console.WriteLine("Nenhuma movimentação registrada até o momento.");
        return;
    }

    foreach (var movimentacao in movimentacoes.OrderByDescending(m => m.Data))
    {
        var produto = dadosEstoque.Estoque.FirstOrDefault(p => p.CodigoProduto == movimentacao.CodigoProduto);
        var nomeProduto = produto != null ? produto.DescricaoProduto : "Produto não encontrado";

        Console.WriteLine($"Código: {movimentacao.CodigoMovimentacao} | {movimentacao.Data:dd/MM/yyyy HH:mm:ss} | {movimentacao.CodigoProduto} - {nomeProduto} | {movimentacao.Tipo} | Quantidade: {movimentacao.Quantidade}");
    }
}

static void ExecutarDesafioJuros()
{
    Console.WriteLine();
    Console.WriteLine("--- DESAFIO 3: CÁLCULO DE JUROS ---");

    Console.Write("Informe o valor original: R$");
    if (!decimal.TryParse(Console.ReadLine(), out decimal valorOriginal) || valorOriginal <= 0)
    {
        Console.WriteLine("Valor inválido. Digite um número maior que zero.");
        return;
    }

    Console.Write("Informe a data de vencimento (dd/MM/yyyy): ");
    if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataVencimento))
    {
        Console.WriteLine("Data inválida. Use o formato dd/MM/yyyy.");
        return;
    }

    var hoje = DateTime.Today;
    var diasAtraso = (hoje - dataVencimento).Days;

    if (diasAtraso < 0)
    {
        Console.WriteLine("A data de vencimento não pode ser futura. Digite uma data anterior ou igual a hoje.");
        return;
    }

    var service = new JurosService();
    var resultado = service.Calcular(valorOriginal, dataVencimento);

    Console.WriteLine($"Valor original: {resultado.ValorOriginal:C2}");
    Console.WriteLine($"Data de vencimento: {dataVencimento:dd/MM/yyyy}");
    Console.WriteLine($"Dias em atraso: {resultado.DiasAtraso}");
    Console.WriteLine($"Juros: {resultado.Juros:C2}");
    Console.WriteLine($"Valor total: {resultado.ValorTotal:C2}");
}

public class EstoqueData
{
    public List<Produto> Estoque { get; set; } = new();
}

public class MovimentacaoData
{
    public List<MovimentacaoEstoque> Movimentacoes { get; set; } = new();
}