using CoisasAFazer.Core.Models;
using System;
using System.Collections.Generic;

namespace CoisasAFazer.Infrastructure
{
    public interface IRepositorioTarefas
    {
        void IncluirTarefas(Tarefa[] tarefas);
        void AtualizarTarefas(Tarefa[] tarefas);
        void ExcluirTarefas(Tarefa[] tarefas);
        Categoria ObtemCategoriaPorId(int id);
        IEnumerable<Tarefa> ObtemTarefas(Func<Tarefa, bool> filtro);
    }
}
