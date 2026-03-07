using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public Double Quantidade { get; set; }
        public Double Preco { get; set; }

        public Double Total { get => Quantidade * Preco;  }
    }
}
