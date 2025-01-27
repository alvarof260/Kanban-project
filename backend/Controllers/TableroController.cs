using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using Kanban.Repositories;
using Kanban.DTO;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TableroController : ControllerBase
{
  private readonly ILogger<TableroController> _logger;
  private ITableroRepository _tableroRepository;

  public TableroController(ILogger<TableroController> logger, ITableroRepository tableroRepository)
  {
    this._logger = logger;
    this._tableroRepository = tableroRepository;
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

  [HttpPost]
  public IActionResult Crear(Tablero tablero)
  {
    try
    {
      Tablero nuevoTablero = _tableroRepository.CrearTablero(tablero);
      return Created("api/Tablero", nuevoTablero);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw new Exception("Error al crear el tablero");
    }
  }

  [HttpPut("{id}")]
  public IActionResult Modificar(int id, [FromBody] TableroDTO tablero)
  {
    try
    {
      _tableroRepository.ModificarTablero(id, tablero);
      return Ok("tablero modificado");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw new Exception("Error al modificar el tablero");
    }
  }

  [HttpDelete("{id}")]
  public IActionResult Eliminar(int id)
  {
    try
    {
      _tableroRepository.EliminarTablero(id);
      return NoContent();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw new Exception("Error al obtener el tablero");
    }
  }

}
