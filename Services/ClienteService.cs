using Microsoft.EntityFrameworkCore;
using TechRepair.Data;
using TechRepair.Models;

namespace TechRepair.Services;

public interface IClienteService
{
    Task<List<Cliente>> GetAllAsync(string? search = null);
    Task<Cliente?> GetByIdAsync(int id);
    Task<Cliente?> GetWithEquipamentosAsync(int id);
    Task<(bool Success, string? Error)> CreateAsync(Cliente cliente);
    Task<(bool Success, string? Error)> UpdateAsync(Cliente cliente);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
    Task<bool> NifExistsAsync(string nif, int? excludeId = null);
}

public class ClienteService : IClienteService
{
    private readonly ApplicationDbContext _context;
    public ClienteService(ApplicationDbContext context) => _context = context;

    public async Task<List<Cliente>> GetAllAsync(string? search = null)
    {
        var q = _context.Clientes.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(c => c.Nome.ToLower().Contains(s)
                          || c.NIF.Contains(s)
                          || (c.Email != null && c.Email.ToLower().Contains(s))
                          || (c.Telefone != null && c.Telefone.Contains(s)));
        }
        return await q.OrderBy(c => c.Nome).ToListAsync();
    }

    public async Task<Cliente?> GetByIdAsync(int id) =>
        await _context.Clientes.FindAsync(id);

    public async Task<Cliente?> GetWithEquipamentosAsync(int id) =>
        await _context.Clientes
            .Include(c => c.Equipamentos)
            .ThenInclude(e => e.Reparacoes)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> NifExistsAsync(string nif, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(nif)) return false;
        return await _context.Clientes.AnyAsync(c => c.NIF == nif && (!excludeId.HasValue || c.Id != excludeId.Value));
    }

    public async Task<(bool Success, string? Error)> CreateAsync(Cliente cliente)
    {
        if (await NifExistsAsync(cliente.NIF))
            return (false, "Já existe um cliente com este NIF.");
        cliente.DataRegisto = DateTime.UtcNow;
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Cliente cliente)
    {
        if (await NifExistsAsync(cliente.NIF, cliente.Id))
            return (false, "Já existe um cliente com este NIF.");
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var c = await _context.Clientes.Include(x => x.Equipamentos).FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return (false, "Cliente não encontrado.");
        if (c.Equipamentos.Any())
            return (false, "Não é possível eliminar o cliente porque possui equipamentos associados.");
        _context.Clientes.Remove(c);
        await _context.SaveChangesAsync();
        return (true, null);
    }
}
