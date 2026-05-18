using DBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    // Vendas = pedidos com status "Pago"
    public class VendasController : Controller
    {
        private readonly PedidoRepository _repo;

        public VendasController(PedidoRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Vendas";
            ViewData["Pagina"] = "Vendas";

            // Reutiliza GetAll — a view pode filtrar, ou podemos trazer todos os pedidos
            var pedidos = _repo.GetAll();
            return View(pedidos);
        }
    }
}
