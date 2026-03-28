using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao {
            get => _descricao;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a descrição"); 
                }

                _descricao = value;
            }
        }

        public Double Quantidade { get; set; }
        public Double Preco { get; set; }
        public Double Total { get => Quantidade * Preco; }   
        public string Categoria { get; set; }
    }
}
