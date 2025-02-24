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
  public IActionResult GetTareaByIdTablero(int id)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      List<GetTareasViewModel> tareas = _tareaRepository.GetTareaByIdTablero(id);

      return Ok(new { success = true, data = tareas });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al obtener tareas por tablero.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpGet("usuario/{id}")]
  public IActionResult GetTareaByIdUsuario(int id)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      List<GetTareasViewModel> tareas = _tareaRepository.GetTareaByIdUsuario(id);

      return Ok(new { success = true, data = tareas });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al obtener tareas por usuario.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpPost("{id}")]
  public IActionResult CreateTarea(int id, CreateTareaViewModel tarea)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      if (!ModelState.IsValid)
        return BadRequest(new
        {
          success = false,
          message = "Los datos enviados no son válidos.",
          errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        });

      Tarea nuevaTarea = _tareaRepository.CreateTarea(id, tarea);

      return Created("api/Usuario", new { success = true, data = nuevaTarea });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al crear tarea.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpPut("{id}")]
  public IActionResult UpdateTarea(int id, UpdateTareaViewModel tarea)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      if (!ModelState.IsValid)
        return BadRequest(new
        {
          success = false,
          message = "Los datos enviados no son válidos.",
          errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        });

      _tareaRepository.UpdateTarea(id, tarea);

      return Ok(new { success = true, message = "Tarea modificada con éxito." });
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al actualizar tarea.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al actualizar tarea.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpPut("asignar/{idTarea}")]
  public IActionResult AssignTarea(int idTarea, UpdateAssignTareaViewModel model)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      _tareaRepository.AssignTarea(model.IdUsuarioAsignado, idTarea);

      return Ok(new { success = true, message = "Tarea modificada con éxito." });
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al actualizar tarea.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al actualizar tarea.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpDelete("{id}")]
  public IActionResult Eliminar(int id)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      _tareaRepository.DeleteTarea(id);

      return NoContent();
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al eliminar usuario.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al eliminar tarea.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }
}
