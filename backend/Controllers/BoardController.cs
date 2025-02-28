using Kanban.Models;
using Kanban.ViewModels;
using Kanban.ViewModels.Response;
using Kanban.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardController : ControllerBase
{
  private readonly ILogger<BoardController> _logger;
  private IBoardRepository _boardRepository;

  public BoardController(ILogger<BoardController> logger, IBoardRepository boardRepository)
  {
    this._logger = logger;
    this._boardRepository = boardRepository;
  }

  [HttpGet]
  public IActionResult GetBoards()
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      List<GetBoardViewModel> boards = _boardRepository.GetBoards();

      return Ok(new ApiResponse<List<GetBoardViewModel>>(true, "Tableros obtenidos con éxito.", boards));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al obtener tableros.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpGet("getByBoardId/{boardId}")]
  public IActionResult GetBoardId(int boardId)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      GetBoardViewModel board = _boardRepository.GetBoardId(boardId);

      return Ok(new ApiResponse<GetBoardViewModel>(true, "Tablero obtenido con éxito.", board));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al obtener tableros.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpGet("getByUserId/{userId}")]
  public IActionResult GetBoardsByUserId(int userId)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      List<GetBoardViewModel> boards = _boardRepository.GetBoardsByUserId(userId);

      return Ok(new ApiResponse<List<GetBoardViewModel>>(true, "Tableros obtenidos con éxito.", boards));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al obtener tableros.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpPost]
  public IActionResult CreateBoard([FromBody] CreateBoardViewModel board)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      if (!ModelState.IsValid)
        return BadRequest(new
        {
          success = false,
          message = "Los datos enviados no son válidos.",
          errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        });

      Board newBoard = _boardRepository.CreateBoard(board);

      return Created("api/Tablero", new ApiResponse<Board>(true, "Tablero creado con éxito", newBoard));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al crear tablero.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpPut("{id}")]
  public IActionResult UpdateBoard(int id, [FromBody] UpdateBoardViewModel board)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      var ownerBoardValidation = ValidateOwnerBoard(id);
      if (ownerBoardValidation != null) return ownerBoardValidation;

      if (!ModelState.IsValid)
        return BadRequest(new
        {
          success = false,
          message = "Los datos enviados no son válidos.",
          errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        });

      _boardRepository.UpdateBoard(id, board);

      return Ok(new { success = true, message = "Usuario modificado con éxito." });
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al actualizar tablero.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al actualizar tablero.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
    }
  }

  [HttpDelete("{id}")]
  public IActionResult DeleteBoard(int id)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      var ownerBoardValidation = ValidateOwnerBoard(id);
      if (ownerBoardValidation != null) return ownerBoardValidation;

      _boardRepository.DeleteBoard(id);

      return StatusCode(200, new { success = true, message = "Usuario eliminado con éxito." });
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al eliminar tablero.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (InvalidOperationException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al eliminar tablero.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al eliminar tableros.");
      return StatusCode(500, new { success = false, message = "Ocurrió un error interno en el servidor." });
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

  private IActionResult ValidateOwnerBoard(int id)
  {
    if (HttpContext.Session.GetInt32("id") != _boardRepository.GetOwnerUserId(id))
      return StatusCode(403, new { success = false, message = "No tienes permisos necesarios." });
    return null;
  }
}
