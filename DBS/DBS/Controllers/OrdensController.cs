using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DBS.Controllers
{
    public class OrdensController : Controller
    {
        public IActionResult Index()
        {
            var ordens = new List<Ordem>
            {
                new Ordem { Id = 4521, ClienteNome = "Ana Costa", Valor = 890, Status = "Pago" },
                new Ordem { Id = 4520, ClienteNome = "Carlos Souza", Valor = 120, Status = "Pendente" }
            };

            return View(ordens);
        }
    }

    public class Ordem
    {
        public int Id { get; set; }
        public string ClienteNome { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
    }
}