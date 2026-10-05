# Desafio Target

Este projeto é um console em C# que organiza os três desafios em um único menu principal, permitindo que o usuário navegue entre as regras de negócio de forma simples e interativa.

## Fluxo principal do programa

Ao iniciar a aplicação, o usuário verá este menu:

=== MENU ===
1 - Desafio 1: Cálculo de comissões  
2 - Desafio 2: Movimentação de estoque  
3 - Desafio 3: Cálculo de juros  
0 - Sair  

A partir dessa tela, o usuário escolhe qual desafio deseja executar.

---

## 1) Desafio 1: Cálculo de comissões

### Funcionalidade
Ao entrar nessa opção, o sistema lê os dados de vendas armazenados em `Data/vendas.json` e calcula a comissão total agrupada por vendedor.

### Regras aplicadas
- valor abaixo de 100: comissão 0%
- valor entre 100 e 499: comissão 1%
- valor igual ou acima de 500: comissão 5%

### Saída esperada
O programa imprime no console uma linha para cada vendedor, com o total de comissão calculado.

---

## 2) Desafio 2: Movimentação de estoque

### Opções do menu

Ao selecionar essa opção, o sistema entra em um submenu:

--- DESAFIO 2: MOVIMENTAÇÃO DE ESTOQUE ---
1 - Registrar movimentação
2 - Ver histórico de movimentações
0 - Voltar

### Opção 1: Registrar movimentação
O sistema lista os produtos cadastrados e seu estoque atual em `Data/estoque.json`.
Após escolher o produto, o sistema solicita os seguintes dados da movimentação:

1. digitar o tipo da movimentação:
   - `E` para entrada
   - `S` para saída
2. informar a quantidade;

Então salva o histórico em `Data/movimentacoes.json` e atualiza o estoque do produto em `Data/estoque.json`.
Cada movimentação recebe um código numérico único, exibido ao registrar e no histórico.

#### Validações do processo
- aceita somente `E` ou `S` para o tipo de movimentação;
- quantidade deve ser maior que zero;
- saída não pode exceder o estoque atual;
- produto deve existir na lista;

### Opção 2: Ver histórico de movimentações
Essa opção lista todas as movimentações já registradas, mostrando:

- data e hora;
- código numérico da movimentação;
- código do produto;
- descrição do produto;
- tipo da movimentação;
- quantidade movimentada.

### Opção 0: Voltar
Retorna ao menu principal.

---

## 3) Desafio 3: Cálculo de juros

### Funcionalidade
O sistema solicita ao usuário:

1. valor original;
2. data de vencimento no formato `dd/MM/yyyy`.

A partir desses dados, o programa calcula:

- dias em atraso com base na data atual;
- juros simples diários calculados com taxa de 2,5% ao dia;
- valor total com juros.

### Validação
- valor deve ser maior que zero;
- data deve estar no formato correto;
- se a data de vencimento for futura ela é rejeitada.

---

## 0) Sair

### Opção do menu

Essa opção encerra o programa.

---

## Persistência

Os dados são armazenados em JSON:

- `Data/vendas.json` — dados de vendas para o desafio 1
- `Data/estoque.json` — produtos e estoque atual para o desafio 2
- `Data/movimentacoes.json` — histórico das movimentações de estoque para o desafio 2

---

## Como executar

Dentro da pasta do projeto, execute:

```bash
dotnet run
```

Ou primeiro compilar:

```bash
dotnet build
```
