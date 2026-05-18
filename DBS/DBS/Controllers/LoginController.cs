using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            // Se já estiver logado, vai direto pro dashboard
            if (HttpContext.Session.GetString("Usuario") != null)
                return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string senha)
        {
            if (email == "admin" && senha == "123")
            {
                HttpContext.Session.SetString("Usuario", email);
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Erro = "Usuário ou senha inválidos.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
