using CoisasAFazer.Core.Commands;
using CoisasAFazer.Core.Models;
using CoisasAFazer.Infrastructure;

namespace CoisasAFazer.Services.Handlers
{
    public class ObtemCategoriaPorIdHandler
    {
        private readonly IRepositorioTarefas _repositorio;

        public ObtemCategoriaPorIdHandler(IRepositorioTarefas repositorio)
        {
            _repositorio = repositorio;
        }

        public Categoria Execute(ObtemCategoriaPorId comando)
        {
            return _repositorio.ObtemCategoriaPorId(comando.IdCategoria);
        }
    }
}
