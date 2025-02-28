using Kanban.Models;
using Kanban.ViewModels;
using Kanban.ViewModels.Response;
using Kanban.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
  private readonly ILogger<UserController> _logger;
  private IUserRepository _userRepository;

  public UserController(ILogger<UserController> logger, IUserRepository userRepository)
  {
    this._logger = logger;
    this._userRepository = userRepository;
  }

  [HttpGet]
  public IActionResult GetUsuarios()
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      List<GetUserViewModel> users = _userRepository.GetUsers();

      return Ok(new ApiResponse<List<GetUserViewModel>>(true, "Usuarios obtenido con éxito.", users));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error al obtener usuarios.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  [HttpPost]
  public IActionResult CreateUser([FromBody] CreateUserViewModel user)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      var roleValidation = ValidateRole();
      if (roleValidation != null) return roleValidation;

      if (!ModelState.IsValid)
      {
        List<string> errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

        return BadRequest(new ApiResponse<List<string>>(false, "Los datos enviados no son validos.", errors));
      }

      User newUser = _userRepository.CreateUser(user);

      return Created("api/User", new ApiResponse<User>(true, "Usuario creado con exito!", newUser));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error al crear usuario.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  [HttpPut("{id}")]
  public IActionResult UpdateUsuario(int id, [FromBody] UpdateUserViewModel user)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      var roleValidation = ValidateRole();
      if (roleValidation != null) return roleValidation;

      if (!ModelState.IsValid)
      {
        List<string> errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

        return BadRequest(new ApiResponse<List<string>>(false, "Los datos enviados no son validos.", errors));
      }

      _userRepository.UpdateUser(id, user);

      return Ok(new ApiResponse<string>(true, "Usuario fue modificado con éxito.", null));
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex, "Al actualizar usuario.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error al modificar usuario.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  [HttpDelete("{id}")]
  public IActionResult DeleteUsuario(int id)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      var roleValidation = ValidateRole();
      if (roleValidation != null) return roleValidation;

      _userRepository.DeleteUser(id);

      return StatusCode(200, new ApiResponse<string>(true, "Usuario fue eliminado con éxito.", null));
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex, "Error al eliminar usuario.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
    catch (InvalidOperationException ex)
    {
      _logger.LogWarning(ex, "Al eliminar usuario.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error al eliminar usuario.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  private IActionResult ValidateSession()
  {
    if (string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
      return Unauthorized(new ApiResponse<string>(false, "No has iniciado sesión.", null));

    return null;
  }

  private IActionResult ValidateRole()
  {
    if (HttpContext.Session.GetInt32("role") != 1)
      return StatusCode(403, new ApiResponse<string>(false, "No tienes permisos necesarios.", null));

    return null;
  }
}
