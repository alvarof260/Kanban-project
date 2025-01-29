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
    if (!ModelState.IsValid)
      return BadRequest(new
      {
        success = false,
        message = "Los datos enviados no son válidos.",
        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
      });

    if (string.IsNullOrEmpty(model.NombreDeUsuario) && string.IsNullOrEmpty(model.Password))
    {
      return BadRequest(new { success = false, message = "Por favor debe ingresar su usuario y contraseña." });
    }

    Usuario usuario = _usuarioRepository.GetUsuarioNombre(model.NombreDeUsuario);

    if (usuario == null || usuario.Password != model.Password)
    {
      _logger.LogInformation("El usuario " + usuario.NombreDeUsuario + "introdujo mal los datos.");
      return Unauthorized(new { success = false, mensaje = "Usuario o contraseña incorrectos." });
    }

    HttpContext.Session.SetInt32("id", usuario.Id);
    HttpContext.Session.SetString("nombre", usuario.NombreDeUsuario);
    HttpContext.Session.SetInt32("rol", Convert.ToInt32(usuario.RolUsuario));
    _logger.LogInformation("El usuario " + usuario.NombreDeUsuario + " ingreso correctamente.");
    return Ok(new { success = true, data = usuario });
  }



}
