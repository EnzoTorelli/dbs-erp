using DBS.Models;

namespace DBS.ViewModels
{
    public class DashboardViewModel
    {
        public decimal ReceitaMes { get; set; }
        public int TotalClientesAtivos { get; set; }
        public int TotalOrdensAbertas { get; set; }
        public int TotalProdutosEstoque { get; set; }
        public List<Pedido> UltimasOrdens { get; set; } = new();
    }
}
