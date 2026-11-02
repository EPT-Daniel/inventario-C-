using System.ComponentModel.DataAnnotations;

namespace TechRepair.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(9)]
    public string NIF { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [StringLength(30)]
    public string? Telefone { get; set; }

    [StringLength(200)]
    public string? Morada { get; set; }

    [StringLength(10)]
    public string? CodigoPostal { get; set; }

    [StringLength(100)]
    public string? Localidade { get; set; }

    public DateTime DataRegisto { get; set; } = DateTime.UtcNow;

    public ICollection<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();
}
