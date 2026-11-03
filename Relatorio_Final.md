# TechRepair - Relatório Final - Fase 3

## Requisitos Implementados

### A - Análise e desenho do sistema

Foram implementados todos os requisitos funcionais (RF01 a RF19) e requisitos técnicos (RT01 a RT08), incluindo:

- Gestão completa de clientes, equipamentos, técnicos e reparações
- Registro de intervenções técnicas com validação de custos
- Dashboard com indicadores dinâmicos do SQLite
- Relatórios filtrados por cliente, técnico e estado
- Regras de integridade referencial (ON DELETE RESTRICT)

### B - Desenvolvimento de software

Foram desenvolvidos todos os componentes da aplicação:

- **7 Controladores**: Home, Clientes, Equipamentos, Tecnicos, Reparacoes, Intervencoes, Relatorios
- **5 Modelos**: Cliente, Equipamento, Tecnico, Reparacao, Intervencao
- **4 ViewModels**: ClienteListViewModel, EquipamentoListViewModel, TecnicoListViewModel, ReparacaoListViewModel
- **6 Serviços**: ClienteService, EquipamentoService, TecnicoService, ReparacaoService, IntervencaoService, DashboardService
- **Entity Framework Core** com SQLite, migrations aplicadas
- Relações 1:N e N:1 configuradas corretamente

### C - Padrões de desenvolvimento

- Injeção de Dependência em todos os controllers
- Interfaces para todos os serviços
- Padrão MVC separado entre Models, Views, Controllers

### D - Documentação do sistema

- 01-Requisitos.md: Requisitos funcionais e técnicos
- 02-Diagrama-ER.md: Diagrama Entidade-Relação
- 03-Modelo-Relacional.md: Modelo relacional detalhado
- 04-Dicionario-Dados.md: Dicionário de dados completo
- 05-Documentacao-Tecnica.md: Documentação técnica da aplicação
- 06-Manual-Utilizador.md: Manual do utilizador com instruções passo a passo

### E - Apresentação da solução

- Aplicação funcional e compilável em ASP.NET Core MVC (.NET 10)
- Interface baseada em Tabler UI com Bootstrap 5
- Dados persistentes no SQLite com integridade referencial
- Estrutura de projeto organizada e consistente