using DBS.Models;
using MySql.Data.MySqlClient;

namespace DBS.Repositories
{
    public class ProdutoRepository
    {
        private readonly string _connectionString;

        public ProdutoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public List<Produto> GetAll()
        {
            var produtos = new List<Produto>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT p.id, p.nome, p.descricao, p.preco, p.estoque, p.id_categoria, c.nome AS categoria_nome
                FROM produto p
                LEFT JOIN categoria c ON c.id = p.id_categoria
                ORDER BY p.nome", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                produtos.Add(new Produto
                {
                    Id           = reader.GetInt32("id"),
                    Nome         = reader.GetString("nome"),
                    Descricao    = reader.IsDBNull(reader.GetOrdinal("descricao"))       ? null : reader.GetString("descricao"),
                    Preco        = reader.GetDecimal("preco"),
                    Estoque      = reader.GetInt32("estoque"),
                    IdCategoria  = reader.IsDBNull(reader.GetOrdinal("id_categoria"))    ? null : reader.GetInt32("id_categoria"),
                    CategoriaNome = reader.IsDBNull(reader.GetOrdinal("categoria_nome")) ? null : reader.GetString("categoria_nome")
                });
            }
            return produtos;
        }

        public int TotalEstoque()
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT COALESCE(SUM(estoque), 0) FROM produto", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Insert(Produto produto)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "INSERT INTO produto (nome, descricao, preco, estoque, id_categoria) VALUES (@nome, @descricao, @preco, @estoque, @idCategoria)",
                conn);
            cmd.Parameters.AddWithValue("@nome",        produto.Nome);
            cmd.Parameters.AddWithValue("@descricao",   produto.Descricao);
            cmd.Parameters.AddWithValue("@preco",       produto.Preco);
            cmd.Parameters.AddWithValue("@estoque",     produto.Estoque);
            cmd.Parameters.AddWithValue("@idCategoria", produto.IdCategoria);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM produto WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        public Produto? GetById(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(@"
        SELECT p.id, p.nome, p.descricao, p.preco, p.estoque, p.id_categoria, c.nome AS categoria_nome
        FROM produto p
        LEFT JOIN categoria c ON c.id = p.id_categoria
        WHERE p.id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Produto
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? null : reader.GetString("descricao"),
                    Preco = reader.GetDecimal("preco"),
                    Estoque = reader.GetInt32("estoque"),
                    IdCategoria = reader.IsDBNull(reader.GetOrdinal("id_categoria")) ? null : reader.GetInt32("id_categoria"),
                    CategoriaNome = reader.IsDBNull(reader.GetOrdinal("categoria_nome")) ? null : reader.GetString("categoria_nome")
                };
            }
            return null;
        }

        public void Update(Produto produto)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "UPDATE produto SET nome=@nome, descricao=@descricao, preco=@preco, estoque=@estoque, id_categoria=@idCategoria WHERE id=@id",
                conn);
            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
            cmd.Parameters.AddWithValue("@preco", produto.Preco);
            cmd.Parameters.AddWithValue("@estoque", produto.Estoque);
            cmd.Parameters.AddWithValue("@idCategoria", produto.IdCategoria);
            cmd.Parameters.AddWithValue("@id", produto.Id);
            cmd.ExecuteNonQuery();
        }
    }
}
