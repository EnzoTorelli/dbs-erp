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
        [HttpPost]
        public IActionResult Excluir(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Editar(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            var cliente = _repo.GetById(id);
            if (cliente == null) return RedirectToAction("Index");
            return Json(cliente);
        }

        [HttpPost]
        public IActionResult Atualizar(Cliente cliente)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            _repo.Update(cliente);
            return RedirectToAction("Index");
        }
    }
}
