namespace DBS.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int? IdCliente { get; set; }
        public string? ClienteNome { get; set; }
        public DateTime DataPedido { get; set; }
        public string? Status { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal Valor { get; set; }
    }
}
