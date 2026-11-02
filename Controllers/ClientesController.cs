using Microsoft.AspNetCore.Mvc;
using TechRepair.Services;
using TechRepair.ViewModels;

namespace TechRepair.Controllers;

public class ClientesController : Controller
{
    private readonly IClienteService _service;
    public ClientesController(IClienteService service) => _service = service;

    public async Task<IActionResult> Index(string? search)
    {
        var items = await _service.GetAllAsync(search);
        var vm = new ClienteListViewModel { Search = search, Items = items };
        ViewData["Title"] = "Clientes";
        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        var c = await _service.GetWithEquipamentosAsync(id);
        if (c == null) return NotFound();
        ViewData["Title"] = "Ficha do Cliente";
        return View(c);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Novo Cliente";
        return View(new Models.Cliente());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Models.Cliente cliente)
    {
        if (!ModelState.IsValid) { ViewData["Title"] = "Novo Cliente"; return View(cliente); }
        var (ok, err) = await _service.CreateAsync(cliente);
        if (!ok) { ModelState.AddModelError(string.Empty, err!); ViewData["Title"] = "Novo Cliente"; return View(cliente); }
        TempData["Success"] = "Cliente registado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var c = await _service.GetByIdAsync(id);
        if (c == null) return NotFound();
        ViewData["Title"] = "Editar Cliente";
        return View(c);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Models.Cliente cliente)
    {
        if (id != cliente.Id) return BadRequest();
        if (!ModelState.IsValid) { ViewData["Title"] = "Editar Cliente"; return View(cliente); }
        var (ok, err) = await _service.UpdateAsync(cliente);
        if (!ok) { ModelState.AddModelError(string.Empty, err!); ViewData["Title"] = "Editar Cliente"; return View(cliente); }
        TempData["Success"] = "Cliente atualizado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var c = await _service.GetByIdAsync(id);
        if (c == null) return NotFound();
        ViewData["Title"] = "Eliminar Cliente";
        return View(c);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var (ok, err) = await _service.DeleteAsync(id);
        if (!ok) { TempData["Error"] = err; return RedirectToAction(nameof(Delete), new { id }); }
        TempData["Success"] = "Cliente eliminado com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}
