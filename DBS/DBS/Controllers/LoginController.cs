using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}