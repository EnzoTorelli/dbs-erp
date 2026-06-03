using DBS.Models;
using DBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteRepository _repo;

        public ClientesController(ClienteRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Clientes";
            ViewData["Pagina"] = "Clientes";

            var clientes = _repo.GetAll();
            return View(clientes);
        }

        [HttpPost]
        public IActionResult Salvar(Cliente cliente)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            _repo.Insert(cliente);
            return RedirectToAction("Index");
        }
    }
}
