using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using Kanban.Repositories;

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

}
