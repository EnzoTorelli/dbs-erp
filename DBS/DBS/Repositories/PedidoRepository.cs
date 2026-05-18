using DBS.Models;
using MySql.Data.MySqlClient;

namespace DBS.Repositories
{
    public class PedidoRepository
    {
        private readonly string _connectionString;

        public PedidoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public List<Pedido> GetAll()
        {
            var pedidos = new List<Pedido>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT p.id, p.id_cliente, c.nome AS cliente_nome, p.data_pedido, p.status,
                       COALESCE(SUM(ip.quantidade * ip.preco_unitario), 0) AS valor_total
                FROM pedido p
                LEFT JOIN cliente c ON c.id = p.id_cliente
                LEFT JOIN item_pedido ip ON ip.id_pedido = p.id
                GROUP BY p.id, p.id_cliente, c.nome, p.data_pedido, p.status
                ORDER BY p.data_pedido DESC", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pedidos.Add(MapPedido(reader));
            }
            return pedidos;
        }

        public List<Pedido> GetUltimas(int quantidade = 5)
        {
            var pedidos = new List<Pedido>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT p.id, p.id_cliente, c.nome AS cliente_nome, p.data_pedido, p.status,
                       COALESCE(SUM(ip.quantidade * ip.preco_unitario), 0) AS valor_total
                FROM pedido p
                LEFT JOIN cliente c ON c.id = p.id_cliente
                LEFT JOIN item_pedido ip ON ip.id_pedido = p.id
                GROUP BY p.id, p.id_cliente, c.nome, p.data_pedido, p.status
                ORDER BY p.data_pedido DESC
                LIMIT @qtd", conn);
            cmd.Parameters.AddWithValue("@qtd", quantidade);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pedidos.Add(MapPedido(reader));
            }
            return pedidos;
        }

        public int CountAbertos()
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "SELECT COUNT(*) FROM pedido WHERE status = 'Pendente' OR status IS NULL", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public decimal ReceitaMes()
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(@"
                SELECT COALESCE(SUM(ip.quantidade * ip.preco_unitario), 0)
                FROM pedido p
                JOIN item_pedido ip ON ip.id_pedido = p.id
                WHERE MONTH(p.data_pedido) = MONTH(CURDATE())
                  AND YEAR(p.data_pedido)  = YEAR(CURDATE())
                  AND p.status = 'Pago'", conn);
            return Convert.ToDecimal(cmd.ExecuteScalar());
        }

        public void Insert(Pedido pedido)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "INSERT INTO pedido (id_cliente, status) VALUES (@idCliente, @status)",
                conn);
            cmd.Parameters.AddWithValue("@idCliente", pedido.IdCliente);
            cmd.Parameters.AddWithValue("@status",    pedido.Status ?? "Pendente");
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            // Remove itens primeiro (FK)
            var cmdItens = new MySqlCommand("DELETE FROM item_pedido WHERE id_pedido = @id", conn);
            cmdItens.Parameters.AddWithValue("@id", id);
            cmdItens.ExecuteNonQuery();

            var cmd = new MySqlCommand("DELETE FROM pedido WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private static Pedido MapPedido(MySqlDataReader reader) => new Pedido
        {
            Id          = reader.GetInt32("id"),
            IdCliente   = reader.IsDBNull(reader.GetOrdinal("id_cliente"))    ? null : reader.GetInt32("id_cliente"),
            ClienteNome = reader.IsDBNull(reader.GetOrdinal("cliente_nome"))  ? "—"  : reader.GetString("cliente_nome"),
            DataPedido  = reader.IsDBNull(reader.GetOrdinal("data_pedido"))   ? DateTime.MinValue : reader.GetDateTime("data_pedido"),
            Status      = reader.IsDBNull(reader.GetOrdinal("status"))        ? "Pendente" : reader.GetString("status"),
            ValorTotal  = reader.GetDecimal("valor_total")
        };
    }
}
