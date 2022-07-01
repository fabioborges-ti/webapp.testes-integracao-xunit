namespace CoisasAFazer.Core.Models
{
    public class Categoria
    {
        public int Id { get; private set; }
        public string Descricao { get; private set; }

        public Categoria(int id, string descricao) : this(descricao)
        {
            Id = id;
        }

        public Categoria(string descricao)
        {
            Descricao = descricao;
        }
    }
}
