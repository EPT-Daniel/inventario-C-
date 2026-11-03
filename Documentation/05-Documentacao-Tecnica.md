# TechRepair — Documentação Técnica

## Descrição Geral

Aplicação TechRepair desenvolvida em ASP.NET Core MVC com Entity Framework Core e SQLite.

## Tecnologias Utilizadas

- C#
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10
- SQLite
- Tabler UI com Bootstrap 5

## Arquitetura MVC

A aplicação segue o padrão Model-View-Controller:
- Controllers: controlam o fluxo de dados
- Views: representação visual (Tabler UI)
- Models: entidades de dados (Cliente, Equipamento, Tecnico, Reparacao, Intervencao)
## Objetivos

- Gestão completa de clientes, equipamentos, técnicos e reparações
- Registro de intervenções técnicas com cálculos de custos
- Dashboard com indicadores dinâmicos do SQLite
- Relatórios filtros por cliente, técnico e estado

## Tecnologias Detalhadas

### C# e .NET 10

A linguagem C# com o .NET 10 fornece tipagem forte, segurança de memória e desempenho elevado para a aplicação web.

### Entity Framework Core 10

ORM que abstrai o acesso ao banco de dados SQLite, permitindo operações CRUD através de LINQ e migrações code-first.

### SQLite

Banco de dados em arquivo único (techrepair.db) com todas as tabelas necessárias para o funcionamento da aplicação. Utiliza chaves estrangeiras com ON DELETE RESTRICT para integridade referencial.

### Tabler UI com Bootstrap 5

Interface de usuário responsiva baseada em Tabler UI e Bootstrap 5, proporcionando uma experiência visual consistente sem a necessidade de customização extensiva de CSS.

## Estrutura de Pastas

O projeto organiza os arquivos da seguinte maneira:

- **Controllers/**: Controladores responsáveis pelas actions da aplicação
- **Models/**: Entidades do domínio (Cliente, Equipamento, Tecnico, Reparacao, Intervencao)
- **ViewModels/**: Modelos específicos para passar dados às Views
- **Data/**: Contexto do Entity Framework (ApplicationDbContext)
- **Services/**: Serviços de negócio com interfaces e implementações
- **Views/**: arquivos Razor (.cshtml) para renderização
- **wwwroot/**: Recursos estáticos (CSS, JS, imagens)
- **Documentation/**: Documentação do projeto (01-06)
- **TechRepair.csproj**: Projeto principal
- **TechRepair.Tests.csproj**: Projeto de testes unitários

## Organização dos Controllers

Os controladores principais são:

1. **HomeController**: Dashboard e actions auxiliares
2. **ClientesController**: CRUD completo de clientes
3. **EquipamentosController**: CRUD de equipamentos com associação a clientes
4. **TecnicosController**: CRUD de técnicos com toggle de ativo
5. **ReparacoesController**: CRUD de reparações com cálculo de custos
6. **IntervencoesController**: Registo de intervenções técnicas
7. **RelatoriosController**: Relatórios filtrados por cliente, técnico e estado

## ViewModels e Models

- **Models**: Representam as entidades do banco de dados com dados annotations para validação e configurações do EF Core.
- **ViewModels**: Utilizados para passagem de dados específicas às Views, evitando expor dados desnecessários. Exemplos: ClienteListViewModel, EquipamentoListViewModel, ReparacaoListViewModel, TecnicoListViewModel.

## Serviços e Regras de Negócio

Cada entidade tem uma interface e uma classe de serviço:

- **IClienteService / ClienteService**: Registo, consulta, edição e eliminação de clientes. Validação de NIF duplicado. Bloqueio de eliminação se houver equipamentos associados.
- **IEquipamentoService / EquipamentoService**: CRUD de equipamentos. Validação de cliente existente. Bloqueio de eliminação se houver reparações associadas.
- **ITecnicoService / TecnicoService**: CRUD de técnicos. Toggle do campo Ativo. Bloqueio de eliminação se houver reparações associadas.
- **IReparacaoService / ReparacaoService**: CRUD de reparações. Validação de estados permitidos (A aguardar diagnóstico, Em reparação, A aguardar peças, Concluída, Entregue ao cliente). Cálculo automático de custo total através das intervenções.
- **IIntervencaoService / IntervencaoService**: Registo de intervenções com validação de custos (negativos, tempo negativo). Associação obrigatória a uma reparação.
- **IDashboardService / DashboardService**: População do viewmodel do dashboard com contagens totais e listas de reparações recentes.

## Cálculo de Custos

O custo total de uma reparação é a soma de todos os custos das intervenções associadas:

```
CustoTotal = Σ (CustoMãoObra_i + CustoPecas_i)
```

Cada intervenção possui:
- CustoMãoObra: Custo da mão de obra (decimal 18,2)
- CustoPecas: Custo das peças (decimal 18,2)
- TempoTrabalho: Tempo em horas (decimal 10,2), opcional

O método `ReparacaoService.GetCustoTotalAsync(int reparacaoId)` executa este cálculo consultando a base de dados.

## Estados da Reparação

As reparações podem estar em um dos seguintes estados:

1. **A aguardar diagnóstico** - Estado inicial após criação da reparação
2. **Em reparação** - Técnico está a trabalhar no equipamento
3. **A aguardar peças** - Aguardando a chegada de peças necessárias
4. **Concluída** - Reparação finalizada, aguardando entrega
5. **Entregue ao cliente** - Equipamento foi entregue ao cliente

A alteração de estado segue a lógica de negócio definida no `ReparacaoService`, que validade se o novo estado é permitido.

## Pesquisas e Filtros

Os utilizadores podem filtrar e pesquisar em varias áreas:

- **Clientes**: Pesquisa por nome, NIF, email ou telefone.
- **Equipamentos**: Pesquisa por tipo, marca, modelo ou número de série, filtros por cliente.
- **Técnicos**: Pesquisa por nome, email ou especialidade. Filtro por ativo/inativo.
- **Reparações**: Pesquisa por descrição da avaria, filtro por cliente, técnico, estado ou equipamento.
- **Intervenções**: Filtro implicitamente pela reparação a qual pertencem.
- **Relatórios**: Filtros combinados por cliente, técnico e estado, com totais gerais.

## Documentação Relacionada

- 01-Requisitos.md: Requisitos funcionais e técnicos
- 02-Diagrama-ER.md: Diagrama Entidade-Relação
- 03-Modelo-Relacional.md: Modelo relacional detalhado
- 04-Dicionario-Dados.md: Dicionário de dados completo
- 06-Manual-Utilizador.md: Manual do utilizador com instruções passo a passo