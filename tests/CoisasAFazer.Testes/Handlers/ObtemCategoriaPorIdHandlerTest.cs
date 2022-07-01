using CoisasAFazer.Core.Commands;
using CoisasAFazer.Infrastructure;
using CoisasAFazer.Services.Handlers;
using Moq;
using Xunit;

namespace CoisasAFazer.Testes.Handlers
{
    public class ObtemCategoriaPorIdHandlerTest
    {
        [Fact]
        public void QuandoIdForExistenteDeveChamarObtemCategoriaPorIdUmaUnicaVez()
        {
            // arrange
            var idCategoria = 20;
            var comando = new ObtemCategoriaPorId(idCategoria);
            var mock = new Mock<IRepositorioTarefas>();
            var repo = mock.Object;
            var handler = new ObtemCategoriaPorIdHandler(repo);

            // act
            handler.Execute(comando);

            // assert 
            mock.Verify(r => r.ObtemCategoriaPorId(idCategoria), Times.Once);
        }
    }
}
