using DBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly ProdutoRepository _repo;

        public ProdutosController(ProdutoRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Produtos";
            ViewData["Pagina"] = "Produtos";

            var produtos = _repo.GetAll();
            return View(produtos);
        }

        [HttpPost]
        public IActionResult Excluir(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
