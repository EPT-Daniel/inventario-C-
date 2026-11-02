# TechRepair

Aplicação empresarial para gestão de reparações de equipamentos informáticos, desenvolvida em ASP.NET Core MVC.

## Tecnologias

- C#
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10
- SQLite (ficheiro 	echrepair.db)
- Tabler UI com Bootstrap 5
- Visual Studio Code

## Funcionalidades (Fases 1 e 2 concluídas)

- Gestão de clientes, equipamentos, técnicos, reparações e intervenções técnicas.
- Dashboard com indicadores dinâmicos do SQLite.
- CRUD completo para todas as entidades.
- Pesquisa e filtros combinados por cliente, técnico e estado.
- Relatórios com valores totais em euros.
- Histórico de reparações por equipamento.
- Base de dados SQLite funcional com migrations aplicadas.
- Documentação de análise e conceção em Documentation/.

## Estrutura do projeto

    Controllers/
    Models/
    ViewModels/
    Data/
      ApplicationDbContext.cs
      Migrations/
    Services/
    Views/
    wwwroot/
    Documentation/
      01-Requisitos.md
      02-Diagrama-ER.md
      03-Modelo-Relacional.md
      04-Dicionario-Dados.md
    appsettings.json
    Program.cs

## Como executar

1. Verificar o SDK .NET instalado:
   `
   dotnet --version
   `
2. Restaurar e compilar:
   `
   dotnet build
   `
3. Aplicar migrations (já aplicadas; repetir se necessário):
   `
   dotnet ef database update
   `
4. Executar:
   `
   dotnet run
   `
5. Aceder a https://localhost:5001 ou http://localhost:5000 (conforme Properties/launchSettings.json).

## Base de dados

- Motor: SQLite
- Ficheiro: 	echrepair.db (na raiz do projeto)
- Tabelas: Clientes, Equipamentos, Tecnicos, Reparacoes, Intervencoes
- Chaves estrangeiras com ON DELETE RESTRICT (sem eliminação em cascata destrutiva).

## Ciclo completo de utilização

Cliente → Equipamento → Reparação → Intervenções → Custo total → Entrega

## Documentação

Consultar a pasta Documentation/ para requisitos, diagrama ER, modelo relacional e dicionário de dados.

## Autor

Projeto académico — Prova prática de Desenvolvimento de Aplicações Informáticas Empresariais (EPT).
