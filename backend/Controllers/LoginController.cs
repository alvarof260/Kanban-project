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
    {
      return BadRequest(ModelState);
    }

    if (string.IsNullOrEmpty(model.NombreDeUsuario) && string.IsNullOrEmpty(model.Password))
    {
      return BadRequest(new { status = "error", mensaje = "Por favor debe ingresar su usuario y contraseña" });
    }

    Usuario usuario = _usuarioRepository.ObtenerUsuarioNombre(model.NombreDeUsuario);

    if (usuario == null || usuario.Password != model.Password)
    {
      return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });
    }
    HttpContext.Session.SetString("nombre", usuario.NombreDeUsuario);
    HttpContext.Session.SetString("rol", Convert.ToString(usuario.RolUsuario));
    _logger.LogInformation("El usuario " + usuario.NombreDeUsuario + " ingreso correctamente");
    return Ok(new { mensaje = "Autenticacion exitosa", usuario });
  }



}
