using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechRepair.Data;
using TechRepair.Models;

namespace TechRepair.Services;

public interface IReparacaoService
{
    Task<List<Reparacao>> GetAllAsync(int? clienteId = null, int? tecnicoId = null, string? estado = null, int? equipamentoId = null);
    Task<Reparacao?> GetByIdAsync(int id);
    Task<Reparacao?> GetWithDetailsAsync(int id);
    Task<(bool Success, string? Error)> CreateAsync(Reparacao reparacao);
    Task<(bool Success, string? Error)> UpdateAsync(Reparacao reparacao);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
    Task<decimal> GetCustoTotalAsync(int reparacaoId);
    Task<string[]> EstadosPermitidosAsync();
}

public class ReparacaoService : IReparacaoService
{
    private readonly ApplicationDbContext _context;
    public ReparacaoService(ApplicationDbContext context) => _context = context;

    public static readonly string[] Estados = new[]
    {
        "A aguardar diagnóstico",
        "Em reparação",
        "A aguardar peças",
        "Concluída",
        "Entregue ao cliente"
    };

    public Task<string[]> EstadosPermitidosAsync() => Task.FromResult(Estados);

    public async Task<List<Reparacao>> GetAllAsync(int? clienteId = null, int? tecnicoId = null, string? estado = null, int? equipamentoId = null)
    {
        var q = _context.Reparacoes
            .Include(r => r.Equipamento).ThenInclude(e => e.Cliente!)
            .Include(r => r.Tecnico)
            .Include(r => r.Intervencoes)
            .AsQueryable();

        if (equipamentoId.HasValue) q = q.Where(r => r.Equipamento != null && r.Equipamento.ClienteId == equipamentoId.Value);
        if (tecnicoId.HasValue) q = q.Where(r => r.TecnicoId == tecnicoId.Value);
        if (!string.IsNullOrWhiteSpace(estado)) q = q.Where(r => r.Estado == estado);
        if (clienteId.HasValue) q = q.Where(r => r.Equipamento != null && r.Equipamento.ClienteId == clienteId.Value);

        return await q.OrderByDescending(r => r.DataEntrada).ToListAsync();
    }

    public async Task<Reparacao?> GetByIdAsync(int id) =>
        await _context.Reparacoes.FindAsync(id);

    public async Task<Reparacao?> GetWithDetailsAsync(int id) =>
        await _context.Reparacoes
            .Include(r => r.Equipamento).ThenInclude(e => e.Cliente!)
            .Include(r => r.Tecnico)
            .Include(r => r.Intervencoes)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<(bool Success, string? Error)> CreateAsync(Reparacao reparacao)
    {
        var eq = await _context.Equipamentos.FindAsync(reparacao.EquipamentoId);
        if (eq == null) return (false, "Equipamento não encontrado.");
        if (reparacao.TecnicoId.HasValue)
        {
            var t = await _context.Tecnicos.FindAsync(reparacao.TecnicoId.Value);
            if (t == null) return (false, "Técnico não encontrado.");
        }
        if (!Estados.Contains(reparacao.Estado))
            reparacao.Estado = Estados[0];
        if (reparacao.DataEntrada == default) reparacao.DataEntrada = DateTime.UtcNow;
        _context.Reparacoes.Add(reparacao);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Reparacao reparacao)
    {
        var eq = await _context.Equipamentos.FindAsync(reparacao.EquipamentoId);
        if (eq == null) return (false, "Equipamento não encontrado.");
        if (reparacao.TecnicoId.HasValue)
        {
            var t = await _context.Tecnicos.FindAsync(reparacao.TecnicoId.Value);
            if (t == null) return (false, "Técnico não encontrado.");
        }
        if (!Estados.Contains(reparacao.Estado))
            return (false, "Estado inválido.");
        _context.Reparacoes.Update(reparacao);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var r = await _context.Reparacoes.Include(x => x.Intervencoes).FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return (false, "Reparação não encontrada.");
        if (r.Intervencoes.Any())
            return (false, "Não é possível eliminar a reparação porque possui intervenções associadas.");
        _context.Reparacoes.Remove(r);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<decimal> GetCustoTotalAsync(int reparacaoId)
    {
        return await _context.Intervencoes
            .Where(i => i.ReparacaoId == reparacaoId)
            .SumAsync(i => i.CustoMaoObra + i.CustoPecas);
    }
}
