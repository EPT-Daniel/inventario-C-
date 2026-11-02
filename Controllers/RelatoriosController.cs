using Microsoft.AspNetCore.Mvc;
using TechRepair.Services;

namespace TechRepair.Controllers;

public class RelatoriosController : Controller
{
    private readonly IReparacaoService _rep;
    private readonly IClienteService _cliente;
    private readonly ITecnicoService _tecnico;
    public RelatoriosController(IReparacaoService rep, IClienteService cliente, ITecnicoService tecnico)
    { _rep = rep; _cliente = cliente; _tecnico = tecnico; }

    public async Task<IActionResult> Index(int? clienteId, int? tecnicoId, string? estado)
    {
        var items = await _rep.GetAllAsync(clienteId, tecnicoId, estado, null);
        ViewBag.Clientes = await _cliente.GetAllAsync();
        ViewBag.Tecnicos = await _tecnico.GetAllAsync();
        ViewBag.Estados = _rep.EstadosPermitidosAsync().Result;
        ViewBag.TotalGeral = items.Sum(r => r.Intervencoes.Sum(i => i.CustoMaoObra + i.CustoPecas));
        ViewData["Title"] = "Relatórios";
        return View(items);
    }
}
