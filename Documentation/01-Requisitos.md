# TechRepair — Requisitos do Sistema

## 1. Descrição Geral

A aplicação **TechRepair** é um sistema de gestão de reparações de equipamentos informáticos,
desenvolvido em ASP.NET Core MVC com Entity Framework Core e base de dados SQLite.

## 2. Requisitos Funcionais

### Gestão de Clientes
- RF01: Registar novos clientes.
- RF02: Consultar, editar e remover clientes.
- RF03: Associar equipamentos a um cliente.
- RF04: Consultar o histórico de reparações de um cliente através dos seus equipamentos.

### Gestão de Equipamentos
- RF05: Registar equipamentos associados a um cliente.
- RF06: Consultar, editar e remover equipamentos.
- RF07: Registar tipo, marca, modelo, número de série e descrição do equipamento.
- RF08: Consultar as reparações associadas a um equipamento.

### Gestão de Técnicos
- RF09: Registar técnicos.
- RF10: Consultar, editar e remover técnicos.
- RF11: Ativar/desativar técnicos (campo Ativo).
- RF12: Consultar as reparações atribuídas a um técnico.

### Gestão de Reparações
- RF13: Criar uma reparação associada a um equipamento e a um técnico (opcional).
- RF14: Registar data de entrada, descrição da avaria, diagnóstico, estado, datas de conclusão/entrega e observações.
- RF15: Alterar o estado da reparação entre os estados permitidos.
- RF16: Consultar, editar e remover reparações.

### Gestão de Intervenções Técnicas
- RF17: Registar intervenções técnicas associadas a uma reparação.
- RF18: Registar data da intervenção, descrição do trabalho, peças utilizadas, tempo de trabalho, custos de mão de obra e peças e observações.
- RF19: Consultar, editar e remover intervenções.

## 3. Requisitos Técnicos

- RT01: Plataforma ASP.NET Core MVC.
- RT02: Persistência com Entity Framework Core.
- RT03: Base de dados SQLite (ficheiro `techrepair.db`).
- RT04: Interface baseada em Tabler UI com Bootstrap 5.
- RT05: Organização em pastas: Controllers, Models, ViewModels, Data, Services, Views, wwwroot, Documentation.
- RT06: Migrations do Entity Framework Core aplicadas.
- RT07: Chaves primárias e estrangeiras configuradas.
- RT08: Restrições de integridade referencial (sem eliminação em cascata destrutiva de históricos).

## 4. Regras de Negócio

- RN01: Um cliente pode possuir vários equipamentos.
- RN02: Um equipamento pertence a um único cliente.
- RN03: Um equipamento pode ter várias reparações ao longo do tempo.
- RN04: Uma reparação pertence a um único equipamento.
- RN05: Uma reparação pode ser atribuída a um técnico (opcional).
- RN06: Um técnico pode estar associado a várias reparações.
- RN07: Uma reparação pode ter várias intervenções técnicas.
- RN08: Uma intervenção pertence a uma única reparação.
- RN09: Os estados permitidos de uma reparação são:
  - A aguardar diagnóstico
  - Em reparação
  - A aguardar peças
  - Concluída
  - Entregue ao cliente
- RN10: Não é permitida a eliminação em cascata de clientes, equipamentos, técnicos, reparações ou intervenções que destrua o histórico.
- RN11: A eliminação de um registo com dependências é bloqueada (comportamento Restrict).
- RN12: Os valores monetários são armazenados com precisão decimal (18,2).
- RN13: O tempo de trabalho é armazenado em horas (decimal).
