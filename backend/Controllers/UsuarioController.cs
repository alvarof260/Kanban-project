using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using Kanban.Repositories;

namespace Kanban.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UsuarioController : ControllerBase
  {
    private readonly ILogger<UsuarioController> _logger;
    private readonly UsuarioRepository _usuarioRepository;

    public UsuarioController(ILogger<UsuarioController> logger)
    {
      _logger = logger;
      _usuarioRepository = new UsuarioRepository(@"Data Source=/mnt/c/Users/alvarof260/Documents/Kanban.db;Cache=Shared");
    }

    [HttpPost]
    public IActionResult CrearUsuario([FromBody] Usuario usuario)
    {
      if (usuario == null || string.IsNullOrWhiteSpace(usuario.NombreDeUsuario))
      {
        return BadRequest("El usuario proporcionado no es válido.");
      }

      try
      {
        _usuarioRepository.CrearUsuario(usuario);
        _logger.LogInformation("Usuario creado correctamente: {NombreDeUsuario}", usuario.NombreDeUsuario);
        return Ok(new { message = "Usuario creado con éxito." });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error al crear el usuario.");
        return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
      }
    }
  }
}
