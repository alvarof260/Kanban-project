using Kanban.Models;
using Kanban.ViewModels;
using Kanban.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
      List<GetTablerosViewModel> tableros = _tableroRepository.GetTableros();
      return Ok(tableros);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw new Exception("Error al obtener los tableros");
    }
  }

  [HttpPost]
  public IActionResult Crear(CreateTableroViewModel tablero)
  {
    try
    {
      Tablero nuevoTablero = _tableroRepository.CreateTablero(tablero);
      return Created("api/Tablero", nuevoTablero);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw new Exception("Error al crear el tablero");
    }
  }

  [HttpPut("{id}")]
  public IActionResult Modificar(int id, [FromBody] UpdateTableroViewModel tablero)
  {
    try
    {
      _tableroRepository.UpdateTablero(id, tablero);
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
      _tableroRepository.DeleteTablero(id);
      return NoContent();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      throw new Exception("Error al obtener el tablero");
    }
  }
}
