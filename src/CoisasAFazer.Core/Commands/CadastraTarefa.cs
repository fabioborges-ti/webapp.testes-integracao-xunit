using CoisasAFazer.Core.Models;
using System;

namespace CoisasAFazer.Core.Commands
{
    public class CadastraTarefa
    {
        public string Titulo { get; }
        public Categoria Categoria { get; }
        public DateTime Prazo { get; }

        public CadastraTarefa(string titulo, Categoria categoria, DateTime prazo)
        {
            Titulo = titulo;
            Categoria = categoria;
            Prazo = prazo;
        }
    }
}
