using Kanban.Models;
using Kanban.ViewModels;
using Kanban.ViewModels.Response;
using Kanban.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
  private readonly ILogger<LoginController> _logger;
  private IUserRepository _userRepository;

  public LoginController(ILogger<LoginController> logger, IUserRepository userRepository)
  {
    this._logger = logger;
    this._userRepository = userRepository;
  }


  [HttpPost]
  public IActionResult Login([FromBody] LoginViewModel model)
  {
    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new ApiResponse<string>(false, "Los datos enviados no son válidos", null));
      }

      if (string.IsNullOrEmpty(model.Username) && string.IsNullOrEmpty(model.Password))
      {
        return BadRequest(new ApiResponse<string>(false, "Por favor  debe ingresar su usuario y contraseña.", null));
      }

      User user = _userRepository.GetUserLogin(model.Username);

      if (user == null || user.Password != model.Password)
      {
        _logger.LogInformation("Intento de acceso invalido - Usuario: " + model.Username + " Clave ingresada: " + model.Password + ".");
        return Unauthorized(new ApiResponse<string>(false, "Usuario o contraseña incorrectos.", null));
      }

      HttpContext.Session.SetString("IsAuthenticated", "true");
      HttpContext.Session.SetInt32("id", user.Id);
      HttpContext.Session.SetString("username", user.Username);
      HttpContext.Session.SetInt32("role", Convert.ToInt32(user.RoleUser));
      _logger.LogInformation("El usuario " + user.Username + " ingreso correctamente.");
      return Ok(new ApiResponse<User>(true, "Usuario ingreso correctamente", user));
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogError("Error al iniciar sesión: " + ex.Message);
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
    catch (Exception ex)
    {
      _logger.LogError("Error al iniciar sesión: " + ex.Message);
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  [HttpPost("logout")]
  public IActionResult LogOut()
  {
    try
    {
      HttpContext.Session.Clear();

      Response.Cookies.Delete(".AspNetCore.Session");

      return Ok(new ApiResponse<string>(true, "Sesión cerrada correctamente.", null));
    }
    catch (Exception ex)
    {
      _logger.LogError("Error al cerrar sesión: " + ex.Message);
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }
}
