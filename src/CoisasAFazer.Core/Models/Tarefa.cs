using System;

namespace CoisasAFazer.Core.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public Categoria Categoria { get; set; }
        public DateTime Prazo { get; set; }
        public DateTime? ConcluidaEm { get; set; }
        public StatusTarefa Status { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Titulo}, {Categoria.Descricao}, {Prazo:dd/MM/yyyy}";
        }
    }
}
