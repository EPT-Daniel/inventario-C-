# TechRepair — Modelo Relacional

## Tabelas

### Clientes
| Coluna | Tipo | PK | FK | Nulável |
|---|---|---|---|---|
| Id | INTEGER | Sim | — | Não |
| Nome | TEXT | Não | — | Não |
| NIF | TEXT | Não | — | Não |
| Email | TEXT | Não | — | Não |
| Telefone | TEXT | Não | — | Sim |
| Morada | TEXT | Não | — | Sim |
| CodigoPostal | TEXT | Não | — | Sim |
| Localidade | TEXT | Não | — | Sim |
| DataRegisto | TEXT | Não | — | Não |

### Equipamentos
| Coluna | Tipo | PK | FK | Nulável |
|---|---|---|---|---|
| Id | INTEGER | Sim | — | Não |
| ClienteId | INTEGER | Não | Clientes(Id) | Não |
| TipoEquipamento | TEXT | Não | — | Não |
| Marca | TEXT | Não | — | Sim |
| Modelo | TEXT | Não | — | Sim |
| NumeroSerie | TEXT | Não | — | Sim |
| Descricao | TEXT | Não | — | Sim |
| DataRegisto | TEXT | Não | — | Não |

### Tecnicos
| Coluna | Tipo | PK | FK | Nulável |
|---|---|---|---|---|
| Id | INTEGER | Sim | — | Não |
| Nome | TEXT | Não | — | Não |
| Email | TEXT | Não | — | Não |
| Telefone | TEXT | Não | — | Sim |
| Especialidade | TEXT | Não | — | Sim |
| Ativo | INTEGER | Não | — | Não |

### Reparacoes
| Coluna | Tipo | PK | FK | Nulável |
|---|---|---|---|---|
| Id | INTEGER | Sim | — | Não |
| EquipamentoId | INTEGER | Não | Equipamentos(Id) | Não |
| TecnicoId | INTEGER | Não | Tecnicos(Id) | Sim |
| DataEntrada | TEXT | Não | — | Não |
| DescricaoAvaria | TEXT | Não | — | Não |
| Diagnostico | TEXT | Não | — | Sim |
| Estado | TEXT | Não | — | Não |
| DataConclusao | TEXT | Não | — | Sim |
| DataEntrega | TEXT | Não | — | Sim |
| Observacoes | TEXT | Não | — | Sim |

### Intervencoes
| Coluna | Tipo | PK | FK | Nulável |
|---|---|---|---|---|
| Id | INTEGER | Sim | — | Não |
| ReparacaoId | INTEGER | Não | Reparacoes(Id) | Não |
| DataIntervencao | TEXT | Não | — | Não |
| DescricaoTrabalho | TEXT | Não | — | Não |
| PecasUtilizadas | TEXT | Não | — | Sim |
| TempoTrabalho | TEXT | Não | — | Sim |
| CustoMaoObra | TEXT | Não | — | Não |
| CustoPecas | TEXT | Não | — | Não |
| Observacoes | TEXT | Não | — | Sim |

## Chaves Primárias
- Clientes.Id
- Equipamentos.Id
- Tecnicos.Id
- Reparacoes.Id
- Intervencoes.Id

## Chaves Estrangeiras
- Equipamentos.ClienteId → Clientes.Id (ON DELETE RESTRICT)
- Reparacoes.EquipamentoId → Equipamentos.Id (ON DELETE RESTRICT)
- Reparacoes.TecnicoId → Tecnicos.Id (ON DELETE RESTRICT)
- Intervencoes.ReparacaoId → Reparacoes.Id (ON DELETE RESTRICT)

## Relações
- Cliente 1:N Equipamento
- Equipamento 1:N Reparacao
- Tecnico 1:N Reparacao
- Reparacao 1:N Intervencao

## Restrições de Integridade Referencial
Todas as chaves estrangeiras estão configuradas com `ON DELETE RESTRICT`,
impedindo a eliminação de registos que possuam dependências, preservando o histórico.
Não são utilizadas eliminações em cascata destrutivas.
