using Microsoft.EntityFrameworkCore;
using TechRepair.Data;
using TechRepair.Models;

namespace TechRepair.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> GetAsync();
}

public class DashboardViewModel
{
    public int TotalClientes { get; set; }
    public int TotalEquipamentos { get; set; }
    public int TotalReparacoes { get; set; }
    public int AguardarDiagnostico { get; set; }
    public int EmReparacao { get; set; }
    public int AguardarPecas { get; set; }
    public int Concluidas { get; set; }
    public int Entregues { get; set; }
    public List<Reparacao> Recentes { get; set; } = new();
}

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;
    public DashboardService(ApplicationDbContext context) => _context = context;

    public async Task<DashboardViewModel> GetAsync()
    {
        var vm = new DashboardViewModel
        {
            TotalClientes = await _context.Clientes.CountAsync(),
            TotalEquipamentos = await _context.Equipamentos.CountAsync(),
            TotalReparacoes = await _context.Reparacoes.CountAsync(),
            AguardarDiagnostico = await _context.Reparacoes.CountAsync(r => r.Estado == "A aguardar diagnóstico"),
            EmReparacao = await _context.Reparacoes.CountAsync(r => r.Estado == "Em reparação"),
            AguardarPecas = await _context.Reparacoes.CountAsync(r => r.Estado == "A aguardar peças"),
            Concluidas = await _context.Reparacoes.CountAsync(r => r.Estado == "Concluída"),
            Entregues = await _context.Reparacoes.CountAsync(r => r.Estado == "Entregue ao cliente"),
            Recentes = await _context.Reparacoes
                .Include(r => r.Equipamento).ThenInclude(e => e.Cliente!)
                .Include(r => r.Tecnico)
                .OrderByDescending(r => r.DataEntrada)
                .Take(10)
                .ToListAsync()
        };
        return vm;
    }
}
