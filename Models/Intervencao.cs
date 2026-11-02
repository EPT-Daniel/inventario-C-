using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TechRepair.Models;

public class Intervencao
{
    public int Id { get; set; }

    [Required]
    public int ReparacaoId { get; set; }

    public DateTime DataIntervencao { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(2000)]
    public string DescricaoTrabalho { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? PecasUtilizadas { get; set; }

    public decimal? TempoTrabalho { get; set; }

    [Precision(18, 2)]
    public decimal CustoMaoObra { get; set; }

    [Precision(18, 2)]
    public decimal CustoPecas { get; set; }

    [StringLength(1000)]
    public string? Observacoes { get; set; }

    public Reparacao? Reparacao { get; set; }
}
