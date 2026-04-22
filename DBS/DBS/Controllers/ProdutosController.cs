using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    public class ProdutosController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Produtos";
            ViewData["Pagina"] = "Produtos";
            return View();
        }
    }
}