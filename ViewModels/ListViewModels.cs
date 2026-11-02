namespace TechRepair.ViewModels;

public class ClienteListViewModel
{
    public string? Search { get; set; }
    public List<TechRepair.Models.Cliente> Items { get; set; } = new();
}

public class EquipamentoListViewModel
{
    public string? Search { get; set; }
    public List<TechRepair.Models.Equipamento> Items { get; set; } = new();
}

public class TecnicoListViewModel
{
    public string? Search { get; set; }
    public List<TechRepair.Models.Tecnico> Items { get; set; } = new();
}

public class ReparacaoListViewModel
{
    public string? Search { get; set; }
    public int? ClienteId { get; set; }
    public int? TecnicoId { get; set; }
    public string? Estado { get; set; }
    public int? EquipamentoId { get; set; }
    public List<TechRepair.Models.Reparacao> Items { get; set; } = new();
    public List<TechRepair.Models.Cliente> Clientes { get; set; } = new();
    public List<TechRepair.Models.Tecnico> Tecnicos { get; set; } = new();
}
