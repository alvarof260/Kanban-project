using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using Kanban.Repositories;
using Kanban.DTO;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
  private readonly ILogger<UsuarioController> _logger;
  private UsuarioRepository _usuarioRepository;

  public UsuarioController(ILogger<UsuarioController> logger)
  {
    _logger = logger;
    this._usuarioRepository = new UsuarioRepository(@"Data Source=/mnt/c/Users/alvarof260/Documents/Kanban.db;Cache=Shared");
  }

  [HttpGet]
  public IActionResult Listar()
  {
    try
    {
      List<Usuario> usuarios = _usuarioRepository.ObtenerUsuarios();
      return Ok(usuarios);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw new Exception("Error al obtener los usuarios");
    }
  }

  [HttpPost]
  public IActionResult Crear(Usuario usuario)
  {
    try
    {
      Usuario nuevoUsuario = _usuarioRepository.CrearUsuario(usuario);
      return Created("api/Usuario", nuevoUsuario);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw new Exception("Error al crear al usuario");
    }
  }

  [HttpPut("/{id}")]
  public IActionResult Modificar(int id, [FromBody] UsuarioDTO usuario)
  {
    try
    {
      _usuarioRepository.ModificarUsuario(id, usuario);
      return Ok("Usuario modificado");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw new Exception("Error al modificar al usuario");
    }
  }

  [HttpDelete("/{id}")]
  public IActionResult Eliminar(int id)
  {
    try
    {
      _usuarioRepository.EliminarUsuario(id);
      return NoContent();
    }
    catch (InvalidOperationException ex) // Captura de excepciones específicas
    {
      // Maneja casos específicos, como cuando el usuario no puede ser eliminado por estar asociado
      _logger.LogWarning($"No se pudo eliminar al usuario {id}: {ex.Message}");
      return BadRequest(new { mensaje = ex.Message }); // Retorna BadRequest con mensaje descriptivo
    }
    catch (Exception ex)
    {
      // Excepción genérica para cualquier otro error
      _logger.LogError($"Error al eliminar al usuario {id}: {ex.ToString()}");
      return StatusCode(500, new { mensaje = "Error interno al intentar eliminar al usuario." }); // Retorna un Internal Server Error
    }
  }
}
