using Microsoft.EntityFrameworkCore;
using TechRepair.Data;
using TechRepair.Models;

namespace TechRepair.Services;

public interface IIntervencaoService
{
    Task<List<Intervencao>> GetByReparacaoAsync(int reparacaoId);
    Task<Intervencao?> GetByIdAsync(int id);
    Task<(bool Success, string? Error, decimal CustoTotal)> CreateAsync(Intervencao intervencao);
    Task<(bool Success, string? Error, decimal CustoTotal)> UpdateAsync(Intervencao intervencao);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
    Task<decimal> GetCustoTotalAsync(int intervencaoId);
}

public class IntervencaoService : IIntervencaoService
{
    private readonly ApplicationDbContext _context;
    public IntervencaoService(ApplicationDbContext context) => _context = context;

    public async Task<List<Intervencao>> GetByReparacaoAsync(int reparacaoId) =>
        await _context.Intervencoes.Where(i => i.ReparacaoId == reparacaoId)
            .OrderByDescending(i => i.DataIntervencao).ToListAsync();

    public async Task<Intervencao?> GetByIdAsync(int id) => await _context.Intervencoes.FindAsync(id);

    public async Task<(bool Success, string? Error, decimal CustoTotal)> CreateAsync(Intervencao intervencao)
    {
        var rep = await _context.Reparacoes.FindAsync(intervencao.ReparacaoId);
        if (rep == null) return (false, "Reparação não encontrada.", 0m);
        if (intervencao.CustoMaoObra < 0 || intervencao.CustoPecas < 0)
            return (false, "Os custos não podem ser negativos.", 0m);
        if (intervencao.TempoTrabalho.HasValue && intervencao.TempoTrabalho < 0)
            return (false, "O tempo de trabalho não pode ser negativo.", 0m);
        if (intervencao.DataIntervencao == default) intervencao.DataIntervencao = DateTime.UtcNow;
        _context.Intervencoes.Add(intervencao);
        await _context.SaveChangesAsync();
        var total = intervencao.CustoMaoObra + intervencao.CustoPecas;
        return (true, null, total);
    }

    public async Task<(bool Success, string? Error, decimal CustoTotal)> UpdateAsync(Intervencao intervencao)
    {
        var existing = await _context.Intervencoes.FindAsync(intervencao.Id);
        if (existing == null) return (false, "Intervenção não encontrada.", 0m);
        if (intervencao.CustoMaoObra < 0 || intervencao.CustoPecas < 0)
            return (false, "Os custos não podem ser negativos.", 0m);
        if (intervencao.TempoTrabalho.HasValue && intervencao.TempoTrabalho < 0)
            return (false, "O tempo de trabalho não pode ser negativo.", 0m);

        existing.DataIntervencao = intervencao.DataIntervencao;
        existing.DescricaoTrabalho = intervencao.DescricaoTrabalho;
        existing.PecasUtilizadas = intervencao.PecasUtilizadas;
        existing.TempoTrabalho = intervencao.TempoTrabalho;
        existing.CustoMaoObra = intervencao.CustoMaoObra;
        existing.CustoPecas = intervencao.CustoPecas;
        existing.Observacoes = intervencao.Observacoes;
        await _context.SaveChangesAsync();
        var total = existing.CustoMaoObra + existing.CustoPecas;
        return (true, null, total);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var i = await _context.Intervencoes.FindAsync(id);
        if (i == null) return (false, "Intervenção não encontrada.");
        _context.Intervencoes.Remove(i);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<decimal> GetCustoTotalAsync(int intervencaoId)
    {
        var i = await _context.Intervencoes.FindAsync(intervencaoId);
        return i == null ? 0m : i.CustoMaoObra + i.CustoPecas;
    }
}
