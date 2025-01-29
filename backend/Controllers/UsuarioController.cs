using Kanban.Models;
using Kanban.ViewModels;
using Kanban.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
  private readonly ILogger<UsuarioController> _logger;
  private IUsuarioRepository _usuarioRepository;

  public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository)
  {
    this._logger = logger;
    this._usuarioRepository = usuarioRepository;
  }

  [HttpGet]
  public IActionResult GetUsuarios()
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      List<GetUsuariosViewModel> usuarios = _usuarioRepository.GetUsuarios();

      return Ok(new { success = true, data = usuarios });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al obtener usuarios.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpPost]
  public IActionResult CreateUsuario(CreateUsuarioViewModel usuario)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      if (HttpContext.Session.GetInt32("rol") != 1)
        return StatusCode(403, new { success = false, message = "No tienes permisos necesarios." });

      if (!ModelState.IsValid)
        return BadRequest(new
        {
          success = false,
          message = "Los datos enviados no son válidos.",
          errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        });

      Usuario nuevoUsuario = _usuarioRepository.CreateUsuario(usuario);

      return Created("api/Usuario", new { success = true, data = nuevoUsuario });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al crear usuario.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpPut("{id}")]
  public IActionResult UpdateUsuario(int id, [FromBody] UpdateUsuarioViewModel usuario)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      if (HttpContext.Session.GetInt32("rol") != 1)
        return StatusCode(403, new { success = false, message = "No tienes permisos necesarios." });

      if (!ModelState.IsValid)
        return BadRequest(new
        {
          success = false,
          message = "Los datos enviados no son válidos.",
          errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        });

      _usuarioRepository.UpdateUsuario(id, usuario);

      return Ok(new { success = true, message = "Usuario modificado con éxito." });
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al actualizar usuario.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al modificar usuario.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpDelete("{id}")]
  public IActionResult DeleteUsuario(int id)
  {
    try
    {
      if (string.IsNullOrEmpty(HttpContext.Session.GetString("nombre")))
        return Unauthorized(new { success = false, message = "No has iniciado sesión." });

      if (HttpContext.Session.GetInt32("rol") != 1)
        return StatusCode(403, new { success = false, message = "No tienes permisos necesarios." });

      _usuarioRepository.DeleteUsuario(id);

      return StatusCode(200, new { success = true, message = "Usuario eliminado con éxito." });
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString());
      return BadRequest(new { success = false, message = ex });
    }
    catch (InvalidOperationException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al eliminar usuario.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al eliminar usuario.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }
}
