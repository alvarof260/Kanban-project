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
  public IActionResult ListarPorTablero(int id)
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

  [HttpGet("usuario/{id}")]
  public IActionResult ListarPorUsuario(int id)
  {
    try
    {
      List<Tarea> tareas = _tareaRepository.ObtenerTareasPorUsuario(id);
      return Ok(tareas);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }

  [HttpPost("crear/{id}")]
  public IActionResult Crear(int id, Tarea tarea)
  {
    try
    {
      Tarea nuevaTarea = _tareaRepository.CrearTarea(id, tarea);
      return Created("api/Tarea/crear/" + id, nuevaTarea);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }

  [HttpPut("{id}")]
  public IActionResult Modificar(int id, TareaDTO tarea)
  {
    try
    {
      _tareaRepository.ModificarTarea(id, tarea);
      return Ok("Tarea modificada");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }
}
