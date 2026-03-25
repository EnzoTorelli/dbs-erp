using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            ViewData["Pagina"] = "Dashboard";
            return View();
        }
    }
}