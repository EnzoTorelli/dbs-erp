using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    public class LoginController : Controller
    {
        // Abre a tela de login
        public IActionResult Index()
        {
            return View();
        }

        // Recebe os dados do formulário
        [HttpPost]
        public IActionResult Index(string email, string senha)
        {
            if (email == "admin" && senha == "123")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }
    }
}