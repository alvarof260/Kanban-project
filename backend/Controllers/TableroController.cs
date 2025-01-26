using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using Kanban.Repositories;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TableroController : ControllerBase
{
  private readonly ILogger<TableroController> _logger;
  private TableroRepository _tableroRepository;

  public TableroController(ILogger<TableroController> logger)
  {
    this._logger = logger;
    this._tableroRepository = new TableroRepository(@"Data Source=/mnt/c/Users/alvarof260/Documents/Kanban.db;Cache=Shared");
  }

  [HttpGet]
  public IActionResult Listar()
  {
    try
    {
      List<Tablero> tableros = _tableroRepository.ObtenerTableros();
      return Ok(tableros);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw new Exception("Error al obtener los tableros");
    }
  }

}
