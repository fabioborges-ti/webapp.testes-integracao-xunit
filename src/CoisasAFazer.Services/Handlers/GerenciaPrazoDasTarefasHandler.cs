using CoisasAFazer.Core.Commands;
using CoisasAFazer.Core.Models;
using CoisasAFazer.Infrastructure;
using System.Linq;

namespace CoisasAFazer.Services.Handlers
{
    public class GerenciaPrazoDasTarefasHandler
    {
        private readonly IRepositorioTarefas _repositorio;

        public GerenciaPrazoDasTarefasHandler(IRepositorioTarefas repositorio)
        {
            _repositorio = repositorio;
        }

        public void Execute(GerenciaPrazoDasTarefas comando)
        {
            var agora = comando.DataHoraAtual;

            var tarefas = _repositorio.ObtemTarefas(t => t.Prazo <= agora && t.Status != StatusTarefa.Concluida).ToList();

            tarefas.ForEach(t => t.Status = StatusTarefa.EmAtraso);

            _repositorio.AtualizarTarefas(tarefas.ToArray());
        }
    }
}
