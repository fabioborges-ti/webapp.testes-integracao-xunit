using CoisasAFazer.Core.Models;
using CoisasAFazer.Infrastructure;
using CoisasAFazer.Services.Handlers;
using CoisasAFazer.WebApp.Controllers;
using CoisasAFazer.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace CoisasAFazer.Testes.Controllers
{
    public class TarefasControllerTest
    {
        [Fact]
        public void ExecutaEndpointCadastraTarefaComSucesso()
        {
            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;

            var contexto = new DbTarefasContext(options);

            contexto.Add(new Categoria(20, "Estudo"));
            contexto.SaveChanges();

            var repo = new RepositorioTarefa(contexto);
            var logger = new Mock<ILogger<CadastraTarefaHandler>>();
            var controller = new TarefasController(repo, logger.Object);

            var model = new CadastraTarefaVM
            {
                IdCategoria = 20,
                Titulo = "Estudar Xunit",
                Prazo = new DateTime(2019, 12, 31)
            };

            // act
            var retorno = controller.EndpointCadastraTarefa(model);

            // assert 
            Assert.IsType<OkResult>(retorno);
        }

        [Fact]
        public void QuandoLancarExcecaoDeveRetornarStatusCode500()
        {
            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(r => r.ObtemCategoriaPorId(20)).Returns(new Categoria(20, "Estudo"));
            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>())).Throws(new Exception("Houve um erro"));

            var repo = mock.Object;

            var logger = new Mock<ILogger<CadastraTarefaHandler>>();
            var controller = new TarefasController(repo, logger.Object);

            var model = new CadastraTarefaVM
            {
                IdCategoria = 20,
                Titulo = "Estudar Xunit",
                Prazo = new DateTime(2019, 12, 31)
            };

            // act
            var retorno = controller.EndpointCadastraTarefa(model);

            // assert 
            Assert.IsType<StatusCodeResult>(retorno);

            var statusCode = (retorno as StatusCodeResult).StatusCode;

            Assert.Equal(500, statusCode);
        }
    }
}
