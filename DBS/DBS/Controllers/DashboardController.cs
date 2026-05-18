using DBS.Repositories;
using DBS.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DBS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ClienteRepository _clienteRepo;
        private readonly ProdutoRepository _produtoRepo;
        private readonly PedidoRepository  _pedidoRepo;

        public DashboardController(
            ClienteRepository clienteRepo,
            ProdutoRepository produtoRepo,
            PedidoRepository  pedidoRepo)
        {
            _clienteRepo = clienteRepo;
            _produtoRepo = produtoRepo;
            _pedidoRepo  = pedidoRepo;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Dashboard";
            ViewData["Pagina"] = "Dashboard";

            var vm = new DashboardViewModel
            {
                ReceitaMes           = _pedidoRepo.ReceitaMes(),
                TotalClientesAtivos  = _clienteRepo.Count(),
                TotalOrdensAbertas   = _pedidoRepo.CountAbertos(),
                TotalProdutosEstoque = _produtoRepo.TotalEstoque(),
                UltimasOrdens        = _pedidoRepo.GetUltimas(5)
            };

            return View(vm);
        }
    }
}
