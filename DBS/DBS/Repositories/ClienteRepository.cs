using DBS.Models;
using MySql.Data.MySqlClient;

namespace DBS.Repositories
{
    public class ClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public List<Cliente> GetAll()
        {
            var clientes = new List<Cliente>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT id, nome, cpf, email, telefone, data_cadastro FROM cliente ORDER BY nome",
                conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clientes.Add(new Cliente
                {
                    Id           = reader.GetInt32("id"),
                    Nome         = reader.GetString("nome"),
                    Cpf          = reader.IsDBNull(reader.GetOrdinal("cpf"))      ? null : reader.GetString("cpf"),
                    Email        = reader.IsDBNull(reader.GetOrdinal("email"))     ? null : reader.GetString("email"),
                    Telefone     = reader.IsDBNull(reader.GetOrdinal("telefone"))  ? null : reader.GetString("telefone"),
                    DataCadastro = reader.GetDateTime("data_cadastro")
                });
            }
            return clientes;
        }

        public int Count()
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT COUNT(*) FROM cliente", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Insert(Cliente cliente)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "INSERT INTO cliente (nome, cpf, email, telefone) VALUES (@nome, @cpf, @email, @telefone)",
                conn);
            cmd.Parameters.AddWithValue("@nome",     cliente.Nome);
            cmd.Parameters.AddWithValue("@cpf",      cliente.Cpf);
            cmd.Parameters.AddWithValue("@email",    cliente.Email);
            cmd.Parameters.AddWithValue("@telefone", cliente.Telefone);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            // Remove itens dos pedidos do cliente primeiro
            var cmdItens = new MySqlCommand(@"
        DELETE ip FROM item_pedido ip
        INNER JOIN pedido p ON p.id = ip.id_pedido
        WHERE p.id_cliente = @id", conn);
            cmdItens.Parameters.AddWithValue("@id", id);
            cmdItens.ExecuteNonQuery();

            // Remove os pedidos do cliente
            var cmdPedidos = new MySqlCommand("DELETE FROM pedido WHERE id_cliente = @id", conn);
            cmdPedidos.Parameters.AddWithValue("@id", id);
            cmdPedidos.ExecuteNonQuery();

            // Remove o cliente
            var cmd = new MySqlCommand("DELETE FROM cliente WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        public Cliente? GetById(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "SELECT id, nome, cpf, email, telefone, data_cadastro FROM cliente WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Cliente
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Cpf = reader.IsDBNull(reader.GetOrdinal("cpf")) ? null : reader.GetString("cpf"),
                    Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    Telefone = reader.IsDBNull(reader.GetOrdinal("telefone")) ? null : reader.GetString("telefone"),
                    DataCadastro = reader.GetDateTime("data_cadastro")
                };
            }
            return null;
        }

        public void Update(Cliente cliente)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "UPDATE cliente SET nome=@nome, cpf=@cpf, email=@email, telefone=@telefone WHERE id=@id",
                conn);
            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
            cmd.Parameters.AddWithValue("@email", cliente.Email);
            cmd.Parameters.AddWithValue("@telefone", cliente.Telefone);
            cmd.Parameters.AddWithValue("@id", cliente.Id);
            cmd.ExecuteNonQuery();
        }
    }
}
