using Kanban.Models;
using Kanban.ViewModels;
using Kanban.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
  private readonly ILogger<LoginController> _logger;
  private IUsuarioRepository _usuarioRepository;

  public LoginController(ILogger<LoginController> logger, IUsuarioRepository usuarioRepository)
  {
    this._logger = logger;
    this._usuarioRepository = usuarioRepository;
  }


  [HttpPost]
  public IActionResult Login([FromBody] LoginViewModel model)
  {
    try
    {
      if (!ModelState.IsValid)
        return BadRequest(new
        {
          success = false,
          message = "Los datos enviados no son válidos.",
        });

      if (string.IsNullOrEmpty(model.NombreDeUsuario) && string.IsNullOrEmpty(model.Password))
      {
        return BadRequest(new { success = false, message = "Por favor debe ingresar su usuario y contraseña." });
      }

      Usuario usuario = _usuarioRepository.GetUsuarioNombre(model.NombreDeUsuario);

      if (usuario == null || usuario.Password != model.Password)
      {
        _logger.LogInformation("Intento de acceso invalido - Usuario: " + model.NombreDeUsuario + " Clave ingresada: " + model.Password + ".");
        return Unauthorized(new { success = false, message = "Usuario o contraseña incorrectos." });
      }

      HttpContext.Session.SetString("IsAuthenticated", "true");
      HttpContext.Session.SetString("nombre", usuario.NombreDeUsuario);
      HttpContext.Session.SetInt32("rol", Convert.ToInt32(usuario.RolUsuario));
      _logger.LogInformation("El usuario " + usuario.NombreDeUsuario + " ingreso correctamente.");
      return Ok(new { success = true, message = "Usuario ingreso correctamente", data = usuario });
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogError("Error al cerrar sesión: " + ex.Message);
      return StatusCode(500, new { success = false, message = ex.Message });
    }
    catch (System.Exception)
    {

      throw;
    }
  }

  [HttpPost("logout")]
  public IActionResult LogOut()
  {
    try
    {
      HttpContext.Session.Clear();

      Response.Cookies.Delete(".AspNetCore.Session");

      return Ok(new { success = true, message = "Sesión cerrada correctamente." });
    }
    catch (Exception ex)
    {
      _logger.LogError("Error al cerrar sesión: " + ex.Message);
      return StatusCode(500, new { success = false, message = "Error al cerrar sesión." });
    }
  }
}
