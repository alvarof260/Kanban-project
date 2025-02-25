using Kanban.Models;
using Kanban.ViewModels;
using Kanban.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TableroController : ControllerBase
{
  private readonly ILogger<TableroController> _logger;
  private ITableroRepository _tableroRepository;

  public TableroController(ILogger<TableroController> logger, ITableroRepository tableroRepository)
  {
    this._logger = logger;
    this._tableroRepository = tableroRepository;
  }

  [HttpGet]
  public IActionResult GetTableros()
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      List<GetTablerosViewModel> tableros = _tableroRepository.GetTableros();

      return Ok(new { success = true, data = tableros });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al obtener tableros.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpGet("tablero/{id}")]
  public IActionResult GetTableroIdBoard(int id)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      GetTablerosViewModel tablero = _tableroRepository.GetTableroId(id);

      return Ok(new { success = true, data = tablero });
    }
    catch (System.Exception)
    {

      throw;
    }
  }

  [HttpGet("{id}")]
  public IActionResult GetTablerosByIdUsuario(int id)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      List<GetTablerosViewModel> tableros = _tableroRepository.GetTablerosIdUsuario(id);

      return Ok(new { success = true, data = tableros });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al obtener tableros.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });

    }
  }

  [HttpPost]
  public IActionResult CreateTablero(CreateTableroViewModel tablero)
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

      Tablero nuevoTablero = _tableroRepository.CreateTablero(tablero);

      return Created("api/Tablero", new { success = true, data = nuevoTablero });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al crear tablero.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpPut("{id}")]
  public IActionResult UpdateTablero(int id, [FromBody] UpdateTableroViewModel tablero)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      if (HttpContext.Session.GetInt32("id") != _tableroRepository.GetIdPropietario(id))
        return StatusCode(403, new { success = false, message = "No tienes permisos necesarios." });

      if (!ModelState.IsValid)
        return BadRequest(new
        {
          success = false,
          message = "Los datos enviados no son válidos.",
          errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        });

      _tableroRepository.UpdateTablero(id, tablero);

      return Ok(new { success = true, message = "Usuario modificado con éxito." });
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al actualizar tablero.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al actualizar tablero.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpDelete("{id}")]
  public IActionResult DeleteTablero(int id)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      if (HttpContext.Session.GetInt32("id") != _tableroRepository.GetIdPropietario(id))
        return StatusCode(403, new { success = false, message = "No tienes permisos necesarios." });

      _tableroRepository.DeleteTablero(id);

      return StatusCode(200, new { success = true, message = "Usuario eliminado con éxito." });
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al eliminar tablero.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (InvalidOperationException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al eliminar tablero.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al eliminar tableros.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }
}
