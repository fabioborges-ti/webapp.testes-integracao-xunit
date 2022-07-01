using CoisasAFazer.Core.Commands;
using CoisasAFazer.Core.Models;
using CoisasAFazer.Infrastructure;
using CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace CoisasAFazer.Testes.Handlers
{
    public class CadastraTarefaHandlerTest
    {
        [Fact]
        public void DadaTarefaComInfoValidaDeveIncluirNoDB()
        {
            // arrange
            var comando = new CadastraTarefa("Estudar Xunit", new Categoria(99, "Estudo"), new DateTime(2019, 12, 31));
            var logger = new Mock<ILogger<CadastraTarefaHandler>>();
            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;

            var contexto = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(contexto);
            var handler = new CadastraTarefaHandler(repo, logger.Object);

            // act
            handler.Execute(comando);

            var tarefa = repo.ObtemTarefas(t => t.Titulo == "Estudar Xunit").FirstOrDefault();

            // assert 
            Assert.NotNull(tarefa);
        }

        [Fact]
        public void QuandoExceptionForLancadaResultadoDeveSerFalse()
        {
            // arrange
            var comando = new CadastraTarefa("Estudar Xunit", new Categoria(999, "Estudo"), new DateTime(2019, 12, 31));
            var logger = new Mock<ILogger<CadastraTarefaHandler>>();
            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(new Exception("Houve um erro na inclus�o de tarefas"));

            var repo = mock.Object;

            var handler = new CadastraTarefaHandler(repo, logger.Object);

            // act
            var resultado = handler.Execute(comando);

            // assert
            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void QuandoExceptionForLancadaDeveLogarMensagemDaExcecao()
        {
            var exception = new Exception("Houve um erro na inclus�o de tarefas");

            // arrange
            var comando = new CadastraTarefa("Estudar Xunit", new Categoria(999, "Estudo"), new DateTime(2019, 12, 31));
            var logger = new Mock<ILogger<CadastraTarefaHandler>>();
            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>())).Throws(exception);

            var repo = mock.Object;

            var handler = new CadastraTarefaHandler(repo, logger.Object);

            // act 
            var resultado = handler.Execute(comando);

            // assert
            logger.Verify(l => l.Log
            (
                LogLevel.Error,                                 // 1 - n�vel de log
                It.IsAny<EventId>(),                            // 2 - identificador do evento
                It.IsAny<object>(),                             // 3 - objeto que ser� logado
                exception,                                      // 4 - exce��o que ser� logada
                It.IsAny<Func<object, Exception, string>>()     // 5 - fun��o que converte objeto + exce��o
            ), Times.Once);
        }

        delegate void CapturaMensagemLog(LogLevel level, EventId eventId, object state, Exception exception, Func<object, Exception, string> function);

        [Fact]
        public void DadaTarefaComInfoValidaDeveLogar()
        {
            // arrange
            var comando = new CadastraTarefa("Estudar Xunit", new Categoria(99, "Estudo"), new DateTime(2019, 12, 31));
            var logger = new Mock<ILogger<CadastraTarefaHandler>>();

            var levelCapturado = LogLevel.None;
            var mensagemCapturada = string.Empty;

            CapturaMensagemLog captura = (level, eventId, state, exception, func) =>
            {
                levelCapturado = level;
                mensagemCapturada = func(state, exception);
            };

            logger.Setup(l => l.Log
            (
                It.IsAny<LogLevel>(),                           // 1 - n�vel de log
                It.IsAny<EventId>(),                            // 2 - identificador do evento
                It.IsAny<object>(),                             // 3 - objeto que ser� logado
                It.IsAny<Exception>(),                          // 4 - exce��o que ser� logada
                It.IsAny<Func<object, Exception, string>>()     // 5 - fun��o que converte objeto + exce��o
            )).Callback(captura);

            var repo = new Mock<IRepositorioTarefas>();
            var handler = new CadastraTarefaHandler(repo.Object, logger.Object);

            // act 
            handler.Execute(comando);

            // assert 
            Assert.Equal(LogLevel.Information, levelCapturado);
            Assert.Contains("Estudar Xunit", mensagemCapturada);
        }
    }
}
