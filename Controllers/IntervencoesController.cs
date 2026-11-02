using Microsoft.AspNetCore.Mvc;
using TechRepair.Services;

namespace TechRepair.Controllers;

public class IntervencoesController : Controller
{
    private readonly IIntervencaoService _service;
    private readonly IReparacaoService _rep;
    public IntervencoesController(IIntervencaoService service, IReparacaoService rep) { _service = service; _rep = rep; }

    public async Task<IActionResult> Create(int reparacaoId)
    {
        var r = await _rep.GetWithDetailsAsync(reparacaoId);
        if (r == null) return NotFound();
        ViewBag.Reparacao = r;
        var iv = new Models.Intervencao { ReparacaoId = reparacaoId, DataIntervencao = DateTime.Now };
        ViewData["Title"] = "Nova Intervenção";
        return View(iv);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Models.Intervencao intervencao)
    {
        if (!ModelState.IsValid) { var r = await _rep.GetWithDetailsAsync(intervencao.ReparacaoId); ViewBag.Reparacao = r; ViewData["Title"] = "Nova Intervenção"; return View(intervencao); }
        var (ok, err, total) = await _service.CreateAsync(intervencao);
        if (!ok) { ModelState.AddModelError(string.Empty, err!); var r = await _rep.GetWithDetailsAsync(intervencao.ReparacaoId); ViewBag.Reparacao = r; ViewData["Title"] = "Nova Intervenção"; return View(intervencao); }
        TempData["Success"] = $"Intervenção registada. Custo total: {total:C2}";
        return RedirectToAction("Details", "Reparacoes", new { id = intervencao.ReparacaoId });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var iv = await _service.GetByIdAsync(id);
        if (iv == null) return NotFound();
        var r = await _rep.GetWithDetailsAsync(iv.ReparacaoId);
        ViewBag.Reparacao = r;
        ViewData["Title"] = "Editar Intervenção";
        return View(iv);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Models.Intervencao intervencao)
    {
        if (id != intervencao.Id) return BadRequest();
        if (!ModelState.IsValid) { var r = await _rep.GetWithDetailsAsync(intervencao.ReparacaoId); ViewBag.Reparacao = r; ViewData["Title"] = "Editar Intervenção"; return View(intervencao); }
        var (ok, err, total) = await _service.UpdateAsync(intervencao);
        if (!ok) { ModelState.AddModelError(string.Empty, err!); var r = await _rep.GetWithDetailsAsync(intervencao.ReparacaoId); ViewBag.Reparacao = r; ViewData["Title"] = "Editar Intervenção"; return View(intervencao); }
        TempData["Success"] = $"Intervenção atualizada. Custo total: {total:C2}";
        return RedirectToAction("Details", "Reparacoes", new { id = intervencao.ReparacaoId });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var iv = await _service.GetByIdAsync(id);
        if (iv == null) return NotFound();
        var r = await _rep.GetWithDetailsAsync(iv.ReparacaoId);
        ViewBag.Reparacao = r;
        ViewData["Title"] = "Eliminar Intervenção";
        return View(iv);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var iv = await _service.GetByIdAsync(id);
        if (iv == null) return NotFound();
        var repId = iv.ReparacaoId;
        var (ok, err) = await _service.DeleteAsync(id);
        if (!ok) { TempData["Error"] = err; return RedirectToAction("Delete", new { id }); }
        TempData["Success"] = "Intervenção eliminada com sucesso.";
        return RedirectToAction("Details", "Reparacoes", new { id = repId });
    }
}
