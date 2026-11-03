using Microsoft.EntityFrameworkCore;
using TechRepair.Data;
using TechRepair.Models;

namespace TechRepair.Services;

public interface ITecnicoService
{
    Task<List<Tecnico>> GetAllAsync(bool? apenasAtivos = null);
    Task<Tecnico?> GetByIdAsync(int id);
    Task<Tecnico?> GetWithReparacoesAsync(int id);
    Task<(bool Success, string? Error)> CreateAsync(Tecnico tecnico);
    Task<(bool Success, string? Error)> UpdateAsync(Tecnico tecnico);
    Task<(bool Success, string? Error)> ToggleAtivoAsync(int id);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
}

public class TecnicoService : ITecnicoService
{
    private readonly ApplicationDbContext _context;
    public TecnicoService(ApplicationDbContext context) => _context = context;

    public async Task<List<Tecnico>> GetAllAsync(bool? apenasAtivos = null)
    {
        var q = _context.Tecnicos.AsQueryable();
        if (apenasAtivos.HasValue) q = q.Where(t => t.Ativo == apenasAtivos.Value);
        return await q.OrderBy(t => t.Nome).ToListAsync();
    }

    public async Task<Tecnico?> GetByIdAsync(int id) => await _context.Tecnicos.FindAsync(id);

    public async Task<Tecnico?> GetWithReparacoesAsync(int id) =>
        await _context.Tecnicos
            .Include(t => t.Reparacoes).ThenInclude(r => r.Equipamento!).ThenInclude(e => e.Cliente!)
            .Include(t => t.Reparacoes).ThenInclude(r => r.Intervencoes!)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<(bool Success, string? Error)> CreateAsync(Tecnico tecnico)
    {
        _context.Tecnicos.Add(tecnico);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Tecnico tecnico)
    {
        _context.Tecnicos.Update(tecnico);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> ToggleAtivoAsync(int id)
    {
        var t = await _context.Tecnicos.FindAsync(id);
        if (t == null) return (false, "Técnico não encontrado.");
        t.Ativo = !t.Ativo;
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var t = await _context.Tecnicos.Include(x => x.Reparacoes).FirstOrDefaultAsync(x => x.Id == id);
        if (t == null) return (false, "Técnico não encontrado.");
        if (t.Reparacoes.Any())
            return (false, "Não é possível eliminar o técnico porque possui reparações associadas. Desative-o.");
        _context.Tecnicos.Remove(t);
        await _context.SaveChangesAsync();
        return (true, null);
    }
}
