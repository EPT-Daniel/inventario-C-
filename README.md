# TechRepair

Aplicação empresarial para gestão de reparações de equipamentos informáticos, desenvolvida em ASP.NET Core MVC.

## Tecnologias

- C#
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10
- SQLite (ficheiro techrepair.db)
- Tabler UI com Bootstrap 5
- Visual Studio Code

## Funcionalidades (Fase 1 - base)

- Gestão de clientes, equipamentos, técnicos, reparações e intervenções técnicas.
- Modelo de dados com 5 entidades e relações 1:N.
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
   dotnet --version
2. Restaurar e compilar:
   dotnet build
3. Aplicar migrations (já aplicadas; repetir se necessário):
   dotnet ef database update
4. Executar:
   dotnet run
5. Aceder a https://localhost:5001 ou http://localhost:5000 (conforme Properties/launchSettings.json).

## Base de dados

- Motor: SQLite
- Ficheiro: techrepair.db (na raiz do projeto)
- Tabelas: Clientes, Equipamentos, Tecnicos, Reparacoes, Intervencoes
- Chaves estrangeiras com ON DELETE RESTRICT (sem eliminação em cascata destrutiva).

## Documentação

Consultar a pasta Documentation/ para requisitos, diagrama ER, modelo relacional e dicionário de dados.

## Autor

Projeto académico - Prova prática de Desenvolvimento de Aplicações Informáticas Empresariais (EPT).
