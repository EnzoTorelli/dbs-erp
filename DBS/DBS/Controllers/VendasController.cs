using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DBS.Controllers
{
    public class VendasController : Controller
    {
        public IActionResult Index()
        {
            var vendas = new List<Venda>
            {
                new Venda { Id = 1001, ClienteNome = "Maria Silva", Valor = 320, Data = DateTime.Now, Status = "Pago" },
                new Venda { Id = 1002, ClienteNome = "João Pereira", Valor = 150, Data = DateTime.Now, Status = "Pendente" }
            };

            return View(vendas);
        }
    }

    public class Venda
    {
        public int Id { get; set; }
        public string ClienteNome { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
    }
}