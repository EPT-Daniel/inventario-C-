using System.ComponentModel.DataAnnotations;

namespace TechRepair.Models;

public class Equipamento
{
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [Required]
    [StringLength(80)]
    public string TipoEquipamento { get; set; } = string.Empty;

    [StringLength(80)]
    public string? Marca { get; set; }

    [StringLength(80)]
    public string? Modelo { get; set; }

    [StringLength(80)]
    public string? NumeroSerie { get; set; }

    [StringLength(500)]
    public string? Descricao { get; set; }

    public DateTime DataRegisto { get; set; } = DateTime.UtcNow;

    public Cliente? Cliente { get; set; }

    public ICollection<Reparacao> Reparacoes { get; set; } = new List<Reparacao>();
}
