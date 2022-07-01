using CoisasAFazer.Core.Commands;
using CoisasAFazer.Infrastructure;
using CoisasAFazer.Services.Handlers;
using CoisasAFazer.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoisasAFazer.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly IRepositorioTarefas _repositorio;
        private readonly ILogger<CadastraTarefaHandler> _logger;

        public TarefasController(IRepositorioTarefas repositorio, ILogger<CadastraTarefaHandler> logger)
        {
            _repositorio = repositorio;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult EndpointCadastraTarefa(CadastraTarefaVM model)
        {
            var cmdObtemCateg = new ObtemCategoriaPorId(model.IdCategoria);
            var categoria = new ObtemCategoriaPorIdHandler(_repositorio).Execute(cmdObtemCateg);
            if (categoria == null)
            {
                return NotFound("Categoria não encontrada");
            }

            var comando = new CadastraTarefa(model.Titulo, categoria, model.Prazo);
            var handler = new CadastraTarefaHandler(_repositorio, _logger);

            var resultado = handler.Execute(comando);

            if (!resultado.IsSuccess) return StatusCode(500);

            return Ok();
        }
    }
}