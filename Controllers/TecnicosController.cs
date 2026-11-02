using Microsoft.AspNetCore.Mvc;
using TechRepair.Services;
using TechRepair.ViewModels;

namespace TechRepair.Controllers;

public class TecnicosController : Controller
{
    private readonly ITecnicoService _service;
    public TecnicosController(ITecnicoService service) => _service = service;

    public async Task<IActionResult> Index(string? search)
    {
        var items = await _service.GetAllAsync();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            items = items.Where(t => t.Nome.ToLower().Contains(s) || t.Email.ToLower().Contains(s) || (t.Especialidade != null && t.Especialidade.ToLower().Contains(s))).ToList();
        }
        var vm = new TecnicoListViewModel { Search = search, Items = items };
        ViewData["Title"] = "Técnicos";
        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        var t = await _service.GetWithReparacoesAsync(id);
        if (t == null) return NotFound();
        ViewData["Title"] = "Ficha do Técnico";
        return View(t);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Novo Técnico";
        return View(new Models.Tecnico());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Models.Tecnico tecnico)
    {
        if (!ModelState.IsValid) { ViewData["Title"] = "Novo Técnico"; return View(tecnico); }
        var (ok, err) = await _service.CreateAsync(tecnico);
        if (!ok) { ModelState.AddModelError(string.Empty, err!); ViewData["Title"] = "Novo Técnico"; return View(tecnico); }
        TempData["Success"] = "Técnico registado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var t = await _service.GetByIdAsync(id);
        if (t == null) return NotFound();
        ViewData["Title"] = "Editar Técnico";
        return View(t);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Models.Tecnico tecnico)
    {
        if (id != tecnico.Id) return BadRequest();
        if (!ModelState.IsValid) { ViewData["Title"] = "Editar Técnico"; return View(tecnico); }
        var (ok, err) = await _service.UpdateAsync(tecnico);
        if (!ok) { ModelState.AddModelError(string.Empty, err!); ViewData["Title"] = "Editar Técnico"; return View(tecnico); }
        TempData["Success"] = "Técnico atualizado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleAtivo(int id)
    {
        var (ok, err) = await _service.ToggleAtivoAsync(id);
        if (!ok) TempData["Error"] = err; else TempData["Success"] = "Estado do técnico atualizado.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var t = await _service.GetByIdAsync(id);
        if (t == null) return NotFound();
        ViewData["Title"] = "Eliminar Técnico";
        return View(t);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var (ok, err) = await _service.DeleteAsync(id);
        if (!ok) { TempData["Error"] = err; return RedirectToAction(nameof(Delete), new { id }); }
        TempData["Success"] = "Técnico eliminado com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}
