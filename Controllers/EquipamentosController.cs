using Microsoft.AspNetCore.Mvc;
using TechRepair.Services;
using TechRepair.ViewModels;

namespace TechRepair.Controllers;

public class EquipamentosController : Controller
{
    private readonly IEquipamentoService _equip;
    private readonly IClienteService _cliente;
    public EquipamentosController(IEquipamentoService equip, IClienteService cliente) { _equip = equip; _cliente = cliente; }

    public async Task<IActionResult> Index(string? search)
    {
        var items = await _equip.GetAllAsync(search);
        var vm = new EquipamentoListViewModel { Search = search, Items = items };
        ViewData["Title"] = "Equipamentos";
        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        var e = await _equip.GetWithReparacoesAsync(id);
        if (e == null) return NotFound();
        ViewData["Title"] = "Ficha do Equipamento";
        return View(e);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Clientes = await _cliente.GetAllAsync();
        ViewData["Title"] = "Novo Equipamento";
        return View(new Models.Equipamento());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Models.Equipamento equipamento)
    {
        if (!ModelState.IsValid) { ViewBag.Clientes = await _cliente.GetAllAsync(); ViewData["Title"] = "Novo Equipamento"; return View(equipamento); }
        var (ok, err) = await _equip.CreateAsync(equipamento);
        if (!ok) { ModelState.AddModelError(string.Empty, err!); ViewBag.Clientes = await _cliente.GetAllAsync(); ViewData["Title"] = "Novo Equipamento"; return View(equipamento); }
        TempData["Success"] = "Equipamento registado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var e = await _equip.GetByIdAsync(id);
        if (e == null) return NotFound();
        ViewBag.Clientes = await _cliente.GetAllAsync();
        ViewData["Title"] = "Editar Equipamento";
        return View(e);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Models.Equipamento equipamento)
    {
        if (id != equipamento.Id) return BadRequest();
        if (!ModelState.IsValid) { ViewBag.Clientes = await _cliente.GetAllAsync(); ViewData["Title"] = "Editar Equipamento"; return View(equipamento); }
        var (ok, err) = await _equip.UpdateAsync(equipamento);
        if (!ok) { ModelState.AddModelError(string.Empty, err!); ViewBag.Clientes = await _cliente.GetAllAsync(); ViewData["Title"] = "Editar Equipamento"; return View(equipamento); }
        TempData["Success"] = "Equipamento atualizado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var e = await _equip.GetByIdAsync(id);
        if (e == null) return NotFound();
        ViewData["Title"] = "Eliminar Equipamento";
        return View(e);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var (ok, err) = await _equip.DeleteAsync(id);
        if (!ok) { TempData["Error"] = err; return RedirectToAction(nameof(Delete), new { id }); }
        TempData["Success"] = "Equipamento eliminado com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}
