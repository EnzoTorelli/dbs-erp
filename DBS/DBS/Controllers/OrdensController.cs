using DBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    public class OrdensController : Controller
    {
        private readonly PedidoRepository _repo;

        public OrdensController(PedidoRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Ordens";
            ViewData["Pagina"] = "Ordens";

            var ordens = _repo.GetAll();
            return View(ordens);
        }

        [HttpPost]
        public IActionResult Excluir(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
