using Microsoft.EntityFrameworkCore;
using TechRepair.Data;
using TechRepair.Models;

namespace TechRepair.Services;

public interface IEquipamentoService
{
    Task<List<Equipamento>> GetAllAsync(string? search = null);
    Task<Equipamento?> GetByIdAsync(int id);
    Task<Equipamento?> GetWithReparacoesAsync(int id);
    Task<(bool Success, string? Error)> CreateAsync(Equipamento equipamento);
    Task<(bool Success, string? Error)> UpdateAsync(Equipamento equipamento);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
}

public class EquipamentoService : IEquipamentoService
{
    private readonly ApplicationDbContext _context;
    public EquipamentoService(ApplicationDbContext context) => _context = context;

    public async Task<List<Equipamento>> GetAllAsync(string? search = null)
    {
        var q = _context.Equipamentos.Include(e => e.Cliente).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(e => (e.Marca != null && e.Marca.ToLower().Contains(s))
                          || (e.Modelo != null && e.Modelo.ToLower().Contains(s))
                          || (e.NumeroSerie != null && e.NumeroSerie.ToLower().Contains(s))
                          || e.TipoEquipamento.ToLower().Contains(s));
        }
        return await q.OrderBy(e => e.Id).ToListAsync();
    }

    public async Task<Equipamento?> GetByIdAsync(int id) =>
        await _context.Equipamentos.Include(e => e.Cliente).FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Equipamento?> GetWithReparacoesAsync(int id) =>
        await _context.Equipamentos
            .Include(e => e.Cliente)
            .Include(e => e.Reparacoes).ThenInclude(r => r.Tecnico)
            .Include(e => e.Reparacoes).ThenInclude(r => r.Intervencoes)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<(bool Success, string? Error)> CreateAsync(Equipamento equipamento)
    {
        var cliente = await _context.Clientes.FindAsync(equipamento.ClienteId);
        if (cliente == null) return (false, "Cliente não encontrado.");
        equipamento.DataRegisto = DateTime.UtcNow;
        _context.Equipamentos.Add(equipamento);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Equipamento equipamento)
    {
        var cliente = await _context.Clientes.FindAsync(equipamento.ClienteId);
        if (cliente == null) return (false, "Cliente não encontrado.");
        _context.Equipamentos.Update(equipamento);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var e = await _context.Equipamentos.Include(x => x.Reparacoes).FirstOrDefaultAsync(x => x.Id == id);
        if (e == null) return (false, "Equipamento não encontrado.");
        if (e.Reparacoes.Any())
            return (false, "Não é possível eliminar o equipamento porque possui reparações associadas.");
        _context.Equipamentos.Remove(e);
        await _context.SaveChangesAsync();
        return (true, null);
    }
}
