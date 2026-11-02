# TechRepair — Dicionário de Dados

## Cliente

| Campo | Tipo | Obrigatório | Chave | Descrição |
|---|---|---|---|---|
| Id | INTEGER | Sim | PK | Identificador único do cliente. |
| Nome | TEXT (150) | Sim | — | Nome completo do cliente. |
| NIF | TEXT (9) | Sim | — | Número de Identificação Fiscal. |
| Email | TEXT (150) | Sim | — | Endereço de correio eletrónico. |
| Telefone | TEXT (30) | Não | — | Número de telefone. |
| Morada | TEXT (200) | Não | — | Morada do cliente. |
| CodigoPostal | TEXT (10) | Não | — | Código postal. |
| Localidade | TEXT (100) | Não | — | Localidade. |
| DataRegisto | TEXT (datetime) | Sim | — | Data de registo do cliente. |

## Equipamento

| Campo | Tipo | Obrigatório | Chave | Descrição |
|---|---|---|---|---|
| Id | INTEGER | Sim | PK | Identificador único do equipamento. |
| ClienteId | INTEGER | Sim | FK | Cliente proprietário do equipamento. |
| TipoEquipamento | TEXT (80) | Sim | — | Tipo de equipamento (ex.: portátil, desktop, impressora). |
| Marca | TEXT (80) | Não | — | Marca do equipamento. |
| Modelo | TEXT (80) | Não | — | Modelo do equipamento. |
| NumeroSerie | TEXT (80) | Não | — | Número de série. |
| Descricao | TEXT (500) | Não | — | Descrição adicional. |
| DataRegisto | TEXT (datetime) | Sim | — | Data de registo do equipamento. |

## Tecnico

| Campo | Tipo | Obrigatório | Chave | Descrição |
|---|---|---|---|---|
| Id | INTEGER | Sim | PK | Identificador único do técnico. |
| Nome | TEXT (150) | Sim | — | Nome completo do técnico. |
| Email | TEXT (150) | Sim | — | Endereço de correio eletrónico. |
| Telefone | TEXT (30) | Não | — | Número de telefone. |
| Especialidade | TEXT (100) | Não | — | Especialidade técnica. |
| Ativo | INTEGER (bool) | Sim | — | Indica se o técnico está ativo. |

## Reparacao

| Campo | Tipo | Obrigatório | Chave | Descrição |
|---|---|---|---|---|
| Id | INTEGER | Sim | PK | Identificador único da reparação. |
| EquipamentoId | INTEGER | Sim | FK | Equipamento em reparação. |
| TecnicoId | INTEGER | Não | FK | Técnico responsável (opcional). |
| DataEntrada | TEXT (datetime) | Sim | — | Data de entrada do equipamento. |
| DescricaoAvaria | TEXT (1000) | Sim | — | Descrição da avaria reportada. |
| Diagnostico | TEXT (2000) | Não | — | Diagnóstico técnico. |
| Estado | TEXT (50) | Sim | — | Estado da reparação (valores permitidos: A aguardar diagnóstico, Em reparação, A aguardar peças, Concluída, Entregue ao cliente). |
| DataConclusao | TEXT (datetime) | Não | — | Data de conclusão da reparação. |
| DataEntrega | TEXT (datetime) | Não | — | Data de entrega ao cliente. |
| Observacoes | TEXT (1000) | Não | — | Observações adicionais. |

## Intervencao

| Campo | Tipo | Obrigatório | Chave | Descrição |
|---|---|---|---|---|
| Id | INTEGER | Sim | PK | Identificador único da intervenção. |
| ReparacaoId | INTEGER | Sim | FK | Reparação à qual a intervenção pertence. |
| DataIntervencao | TEXT (datetime) | Sim | — | Data da intervenção. |
| DescricaoTrabalho | TEXT (2000) | Sim | — | Descrição do trabalho realizado. |
| PecasUtilizadas | TEXT (1000) | Não | — | Peças utilizadas. |
| TempoTrabalho | TEXT (decimal 10,2) | Não | — | Tempo de trabalho em horas. |
| CustoMaoObra | TEXT (decimal 18,2) | Sim | — | Custo da mão de obra. |
| CustoPecas | TEXT (decimal 18,2) | Sim | — | Custo das peças. |
| Observacoes | TEXT (1000) | Não | — | Observações adicionais. |
