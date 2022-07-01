using CoisasAFazer.Core.Commands;
using CoisasAFazer.Core.Models;
using CoisasAFazer.Infrastructure;
using CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CoisasAFazer.Testes.Handlers
{
    public class GerenciaPrazoDasTarefasHandlerTest
    {
        [Fact]
        public void QuandoTarefasEstiveremAtrasadasDeveMudarStatus()
        {
            // arrange
            var compCateg = new Categoria(11, "Compras");
            var casaCateg = new Categoria(12, "Casa");
            var trabCateg = new Categoria(13, "Trabalho");
            var saudCateg = new Categoria(14, "Saúde");
            var higiCateg = new Categoria(15, "Higiene");

            var listaTarefas = new List<Tarefa>
            {
                // atrasadas
                new Tarefa
                {
                    Titulo = "Tirar lixo",
                    Categoria = casaCateg,
                    Prazo = new DateTime(2018, 12, 31),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
                new Tarefa
                {
                    Titulo = "Fazer o almoço",
                    Categoria = casaCateg,
                    Prazo = new DateTime(2017, 12, 1),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
                new Tarefa
                {
                    Titulo = "Ir à academia",
                    Categoria = saudCateg,
                    Prazo = new DateTime(2018, 12, 31),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
                new Tarefa
                {
                    Titulo = "Concluir o relatório",
                    Categoria = trabCateg,
                    Prazo = new DateTime(2018, 5, 7),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Pendente,
                },
                new Tarefa
                {
                    Titulo = "beber água",
                    Categoria = saudCateg,
                    Prazo = new DateTime(2018, 12, 31),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
                // dentro do prazo
                new Tarefa
                {
                    Titulo = "Comparecer a reunião",
                    Categoria = trabCateg,
                    Prazo = new DateTime(2018, 11, 12),
                    ConcluidaEm = new DateTime(2018, 11, 30),
                    Status = StatusTarefa.Concluida,
                },
                new Tarefa
                {
                    Titulo = "Arrumar a casa",
                    Categoria = casaCateg,
                    Prazo = new DateTime(2019, 4, 5),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
                new Tarefa
                {
                    Titulo = "Escovar os dentes",
                    Categoria = higiCateg,
                    Prazo = new DateTime(2019, 1, 2),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
                new Tarefa
                {
                    Titulo = "Comprar presente pro João",
                    Categoria = compCateg,
                    Prazo = new DateTime(2019, 10, 8),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
                new Tarefa
                {
                    Titulo = "Comprar ração",
                    Categoria = compCateg,
                    Prazo = new DateTime(2019, 11, 20),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
            };

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;

            var contexto = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(contexto);

            repo.IncluirTarefas(listaTarefas.ToArray());

            var comando = new GerenciaPrazoDasTarefas(new DateTime(2019, 1, 1));
            var handler = new GerenciaPrazoDasTarefasHandler(repo);

            // act 
            handler.Execute(comando);

            // assert
            var tarefasEmAtraso = repo.ObtemTarefas(t => t.Status == StatusTarefa.EmAtraso);
            Assert.Equal(5, tarefasEmAtraso.Count());
        }

        [Fact]
        public void QuandoInvocadoDeveChamarAtualizarTarefasNaQtdeVezesDeTarefasAtrasadas()
        {
            // arrange
            var categ = new Categoria("Dummy");

            var tarefas = new List<Tarefa>
            {
                new Tarefa
                {
                    Titulo = "Tirar lixo",
                    Categoria = categ,
                    Prazo = new DateTime(2018, 12, 31),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
                new Tarefa
                {
                    Titulo = "Fazer o almoço",
                    Categoria = categ,
                    Prazo = new DateTime(2017, 12, 1),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
                new Tarefa
                {
                    Titulo = "Ir à academia",
                    Categoria = categ,
                    Prazo = new DateTime(2018, 12, 31),
                    ConcluidaEm = null,
                    Status = StatusTarefa.Criada,
                },
            };

            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.ObtemTarefas(It.IsAny<Func<Tarefa, bool>>())).Returns(tarefas);

            var repo = mock.Object;
            var comando = new GerenciaPrazoDasTarefas(new DateTime(2019, 1, 1));
            var handler = new GerenciaPrazoDasTarefasHandler(repo);

            // act 
            handler.Execute(comando);

            // assert
            mock.Verify(r => r.AtualizarTarefas(It.IsAny<Tarefa[]>()), Times.Once);
        }
    }
}
