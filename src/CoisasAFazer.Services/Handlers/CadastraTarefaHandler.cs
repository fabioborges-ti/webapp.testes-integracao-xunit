using CoisasAFazer.Core.Commands;
using CoisasAFazer.Core.Models;
using CoisasAFazer.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CoisasAFazer.Services.Handlers
{
    public class CadastraTarefaHandler
    {
        private readonly IRepositorioTarefas _repositorio;
        private readonly ILogger<CadastraTarefaHandler> _logger;

        public CadastraTarefaHandler(IRepositorioTarefas repositorio, ILogger<CadastraTarefaHandler> logger)
        {
            _repositorio = repositorio;
            _logger = logger;
        }

        public CommandResult Execute(CadastraTarefa comando)
        {
            try
            {
                var tarefa = new Tarefa
                {
                    Titulo = comando.Titulo,
                    Categoria = comando.Categoria,
                    Prazo = comando.Prazo,
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada
                };

                _logger.LogInformation($"Persistindo a tarefa {tarefa.Titulo}");

                var tarefas = new List<Tarefa> { tarefa };

                _repositorio.IncluirTarefas(tarefas.ToArray());

                return new CommandResult(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return new CommandResult(false);
            }
        }
    }
}
