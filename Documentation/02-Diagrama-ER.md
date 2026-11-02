# TechRepair — Diagrama Entidade-Relação

## Diagrama ER (Mermaid)

```mermaid
erDiagram
    CLIENTE ||--o{ EQUIPAMENTO : possui
    EQUIPAMENTO ||--o{ REPARACAO : "tem"
    TECNICO ||--o{ REPARACAO : "executa"
    REPARACAO ||--o{ INTERVENCAO : "tem"

    CLIENTE {
        int Id PK
        string Nome
        string NIF
        string Email
        string Telefone
        string Morada
        string CodigoPostal
        string Localidade
        datetime DataRegisto
    }

    EQUIPAMENTO {
        int Id PK
        int ClienteId FK
        string TipoEquipamento
        string Marca
        string Modelo
        string NumeroSerie
        string Descricao
        datetime DataRegisto
    }

    TECNICO {
        int Id PK
        string Nome
        string Email
        string Telefone
        string Especialidade
        bool Ativo
    }

    REPARACAO {
        int Id PK
        int EquipamentoId FK
        int TecnicoId FK
        datetime DataEntrada
        string DescricaoAvaria
        string Diagnostico
        string Estado
        datetime DataConclusao
        datetime DataEntrega
        string Observacoes
    }

    INTERVENCAO {
        int Id PK
        int ReparacaoId FK
        datetime DataIntervencao
        string DescricaoTrabalho
        string PecasUtilizadas
        decimal TempoTrabalho
        decimal CustoMaoObra
        decimal CustoPecas
        string Observacoes
    }
```

## Entidades e Cardinalidades

| Relação | Cardinalidade | Descrição |
|---|---|---|
| Cliente — Equipamento | 1 : N | Um cliente possui vários equipamentos. |
| Equipamento — Reparacao | 1 : N | Um equipamento pode ter várias reparações. |
| Tecnico — Reparacao | 1 : N | Um técnico pode estar associado a várias reparações. |
| Reparacao — Intervencao | 1 : N | Uma reparação pode ter várias intervenções técnicas. |
