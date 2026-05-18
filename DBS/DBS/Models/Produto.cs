namespace DBS.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public int? IdCategoria { get; set; }
        public string? CategoriaNome { get; set; }

        public string StatusEstoque => Estoque == 0 ? "indisponivel"
                                     : Estoque <= 5 ? "baixo-estoque"
                                     : "disponivel";

        public string StatusEstoqueLabel => Estoque == 0 ? "Indisponível"
                                           : Estoque <= 5 ? "Baixo Estoque"
                                           : "Disponível";
    }
}
