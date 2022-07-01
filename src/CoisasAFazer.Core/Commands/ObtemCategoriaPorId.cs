using CoisasAFazer.Core.Models;
using MediatR;

namespace CoisasAFazer.Core.Commands
{
    public class ObtemCategoriaPorId : IRequest<Categoria>
    {
        public int IdCategoria { get; }

        public ObtemCategoriaPorId(int idCategoria)
        {
            IdCategoria = idCategoria;
        }
    }
}
