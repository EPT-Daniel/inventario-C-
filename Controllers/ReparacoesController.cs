using Microsoft.AspNetCore.Mvc;
using TechRepair.Services;
using TechRepair.ViewModels;

namespace TechRepair.Controllers;

public class ReparacoesController : Controller
{
    private readonly IReparacaoService _rep;
    private readonly IEquipamentoService _equip;
    private readonly IClienteService _cliente;
    private readonly ITecnicoService _tecnico;
    private readonly IIntervencaoService _interv;
    public ReparacoesController(IReparacaoService rep, IEquipamentoService equip, IClienteService cliente, ITecnicoService tecnico, IIntervencaoService interv)
    { _rep = rep; _equip = equip; _cliente = cliente; _tecnico = tecnico; _interv = interv; }

    public async Task<IActionResult> Index(int? clienteId, int? tecnicoId, string? estado, int? equipamentoId, string? search)
    {
        var items = await _rep.GetAllAsync(clienteId, tecnicoId, estado, equipamentoId);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            items = items.Where(r => (r.Equipamento != null && r.Equipamento.TipoEquipamento.ToLower().Contains(s))
                || (r.Equipamento != null && r.Equipamento.Cliente != null && r.Equipamento.Cliente.Nome.ToLower().Contains(s))
                || r.DescricaoAvaria.ToLower().Contains(s)).ToList();
        }
        var vm = new ReparacaoListViewModel
        {
            Search = search, ClienteId = clienteId, TecnicoId = tecnicoId, Estado = estado, EquipamentoId = equipamentoId,
            Items = items,
            Clientes = await _cliente.GetAllAsync(),
            Tecnicos = await _tecnico.GetAllAsync()
        };
        ViewData["Title"] = "Reparações";
        ViewData["Estados"] = _rep.EstadosPermitidosAsync().Result;
        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        var r = await _rep.GetWithDetailsAsync(id);
        if (r == null) return NotFound();
        ViewData["Title"] = "Detalhe da Reparação";
        ViewData["CustoTotal"] = await _rep.GetCustoTotalAsync(id);
        return View(r);
    }

    public async Task<IActionResult> Create(int? equipamentoId)
    {
        ViewBag.Equipamentos = await _equip.GetAllAsync();
        ViewBag.Tecnicos = await _tecnico.GetAllAsync(true);
        var rep = new Models.Reparacao();
        if (equipamentoId.HasValue) rep.EquipamentoId = equipamentoId.Value;
        rep.DataEntrada = DateTime.Now;
        rep.Estado = _rep.EstadosPermitidosAsync().Result[0];
        ViewData["Title"] = "Nova Reparação";
        return View(rep);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Models.Reparacao reparacao)
    {
        if (!ModelState.IsValid) { ViewBag.Equipamentos = await _equip.GetAllAsync(); ViewBag.Tecnicos = await _tecnico.GetAllAsync(true); ViewData["Title"] = "Nova Reparação"; return View(reparacao); }
        var (ok, err) = await _rep.CreateAsync(reparacao);
        if (!ok) { ModelState.AddModelError(string.Empty, err!); ViewBag.Equipamentos = await _equip.GetAllAsync(); ViewBag.Tecnicos = await _tecnico.GetAllAsync(true); ViewData["Title"] = "Nova Reparação"; return View(reparacao); }
        TempData["Success"] = "Reparação registada com sucesso.";
        return RedirectToAction(nameof(Details), new { id = reparacao.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var r = await _rep.GetWithDetailsAsync(id);
        if (r == null) return NotFound();
        ViewBag.Equipamentos = await _equip.GetAllAsync();
        ViewBag.Tecnicos = await _tecnico.GetAllAsync();
        ViewData["Title"] = "Editar Reparação";
        ViewData["Estados"] = _rep.EstadosPermitidosAsync().Result;
        return View(r);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Models.Reparacao reparacao)
    {
        if (id != reparacao.Id) return BadRequest();
        if (!ModelState.IsValid) { ViewBag.Equipamentos = await _equip.GetAllAsync(); ViewBag.Tecnicos = await _tecnico.GetAllAsync(); ViewData["Title"] = "Editar Reparação"; ViewData["Estados"] = _rep.EstadosPermitidosAsync().Result; return View(reparacao); }
        var (ok, err) = await _rep.UpdateAsync(reparacao);
        if (!ok) { ModelState.AddModelError(string.Empty, err!); ViewBag.Equipamentos = await _equip.GetAllAsync(); ViewBag.Tecnicos = await _tecnico.GetAllAsync(); ViewData["Title"] = "Editar Reparação"; ViewData["Estados"] = _rep.EstadosPermitidosAsync().Result; return View(reparacao); }
        TempData["Success"] = "Reparação atualizada com sucesso.";
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var r = await _rep.GetWithDetailsAsync(id);
        if (r == null) return NotFound();
        ViewData["Title"] = "Eliminar Reparação";
        return View(r);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var (ok, err) = await _rep.DeleteAsync(id);
        if (!ok) { TempData["Error"] = err; return RedirectToAction(nameof(Delete), new { id }); }
        TempData["Success"] = "Reparação eliminada com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}
