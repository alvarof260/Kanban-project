using Kanban.Models;
using Kanban.ViewModels;
using Kanban.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareaController : ControllerBase
{
  private readonly ILogger<TareaController> _logger;
  private ITareaRepository _tareaRepository;

  public TareaController(ILogger<TareaController> logger, ITareaRepository tareaRepository)
  {
    this._logger = logger;
    this._tareaRepository = tareaRepository;
  }

  [HttpGet("tablero/{id}")]
  public IActionResult ListarPorTablero(int id)
  {
    try
    {
      List<GetTareasViewModel> tareas = _tareaRepository.GetTareaByIdTablero(id);
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
      List<GetTareasViewModel> tareas = _tareaRepository.GetTareaByIdUsuario(id);
      return Ok(tareas);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }

  [HttpPost("crear/{id}")]
  public IActionResult Crear(int id, CreateTareaViewModel tarea)
  {
    try
    {
      Tarea nuevaTarea = _tareaRepository.CreateTarea(id, tarea);
      return Created("api/Tarea/crear/" + id, nuevaTarea);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }

  [HttpPut("{id}")]
  public IActionResult Modificar(int id, UpdateTareaViewModel tarea)
  {
    try
    {
      _tareaRepository.UpdateTarea(id, tarea);
      return Ok("Tarea modificada");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }

  [HttpDelete("{id}")]
  public IActionResult Eliminar(int id)
  {
    try
    {
      _tareaRepository.DeleteTarea(id);
      return NoContent();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw;
    }
  }
}
