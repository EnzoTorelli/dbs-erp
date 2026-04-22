using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Cliente";
            ViewData["Pagina"] = "Clientes";
            return View();
        }
    }
}
