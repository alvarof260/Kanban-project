using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using Kanban.Repositories;
using Kanban.DTO;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareaController : ControllerBase
{
  private readonly ILogger<TareaController> _logger;
  private TareaRepository _tareaRepository;

  public TareaController(ILogger<TareaController> logger)
  {
    this._logger = logger;
    this._tareaRepository = new TareaRepository(@"Data Source=/mnt/c/Users/alvarof260/Documents/Kanban.db;Cache=Shared");
  }

  [HttpGet("tablero/{id}")]
  public IActionResult Listar(int id)
  {
    try
    {
      List<Tarea> tareas = _tareaRepository.ObtenerTareaPorTablero(id);
      return Ok(tareas);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }
}
