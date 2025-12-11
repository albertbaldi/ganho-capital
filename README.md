# Ganho de Capital

Calculadora de impostos sobre operações de ações.

## Como usar

```bash
dotnet build
dotnet run --project src/GanhoCapital
```

Entrada via stdin (JSON), linha vazia encerra.

## Exemplo

Entrada:

```json
[
  { "operation": "buy", "unit-cost": 10.0, "quantity": 100 },
  { "operation": "sell", "unit-cost": 15.0, "quantity": 50 }
]
```

Saída:

```json
[{ "tax": 0.0 }, { "tax": 0.0 }]
```

## Testes

```bash
dotnet test
```

## Estrutura

```
src/GanhoCapital/              # Projeto principal
tests/GanhoCapital.Tests/      # Testes unitários
```
