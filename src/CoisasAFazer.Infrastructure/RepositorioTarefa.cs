using CoisasAFazer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoisasAFazer.Infrastructure
{
    public class RepositorioTarefa : IRepositorioTarefas
    {
        private readonly DbTarefasContext _context;

        public RepositorioTarefa(DbTarefasContext context)
        {
            _context = context;
        }

        public void AtualizarTarefas(Tarefa[] tarefas)
        {
            _context.Tarefas.UpdateRange(tarefas);
            _context.SaveChanges();
        }

        public void ExcluirTarefas(Tarefa[] tarefas)
        {
            _context.Tarefas.RemoveRange(tarefas);
            _context.SaveChanges();
        }

        public void IncluirTarefas(Tarefa[] tarefas)
        {
            _context.Tarefas.AddRange(tarefas);
            _context.SaveChanges();
        }

        public Categoria ObtemCategoriaPorId(int id)
        {
            return _context.Categorias.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Tarefa> ObtemTarefas(Func<Tarefa, bool> filtro)
        {
            return _context.Tarefas.Where(filtro);
        }
    }
}
