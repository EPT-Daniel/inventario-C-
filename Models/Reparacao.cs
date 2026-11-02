using System.ComponentModel.DataAnnotations;

namespace TechRepair.Models;

public class Reparacao
{
    public int Id { get; set; }

    [Required]
    public int EquipamentoId { get; set; }

    public int? TecnicoId { get; set; }

    public DateTime DataEntrada { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(1000)]
    public string DescricaoAvaria { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Diagnostico { get; set; }

    [Required]
    [StringLength(50)]
    public string Estado { get; set; } = "A aguardar diagnóstico";

    public DateTime? DataConclusao { get; set; }

    public DateTime? DataEntrega { get; set; }

    [StringLength(1000)]
    public string? Observacoes { get; set; }

    public Equipamento? Equipamento { get; set; }

    public Tecnico? Tecnico { get; set; }

    public ICollection<Intervencao> Intervencoes { get; set; } = new List<Intervencao>();
}
