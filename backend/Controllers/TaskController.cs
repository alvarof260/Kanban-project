using Kanban.Models;
using Kanban.ViewModels;
using Kanban.ViewModels.Response;
using Kanban.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
  private readonly ILogger<TaskController> _logger;
  private ITaskRepository _taskRepository;

  public TaskController(ILogger<TaskController> logger, ITaskRepository taskRepository)
  {
    this._logger = logger;
    this._taskRepository = taskRepository;
  }

  [HttpGet("GetTasksByBoardId/{boardId}")]
  public IActionResult GetTasksByBoardId(int boardId)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      List<GetTaskViewModel> tasks = _taskRepository.GetTasksByBoardId(boardId);

      return Ok(new ApiResponse<List<GetTaskViewModel>>(true, "Tareas obtenida con éxito.", tasks));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al obtener tareas por tablero.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  [HttpGet("GetTasksByUserId/{userId}")]
  public IActionResult GetTasksByUserId(int userId)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      List<GetTaskViewModel> tasks = _taskRepository.GetTasksByUserId(userId);

      return Ok(new ApiResponse<List<GetTaskViewModel>>(true, "Tareas obtenida con éxito.", tasks));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al obtener tareas por usuario.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  [HttpPost("{id}")]
  public IActionResult CreateTask(int id, CreateTaskViewModel task)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      if (!ModelState.IsValid)
      {
        List<string> errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

        return BadRequest(new ApiResponse<List<string>>(false, "Los datos enviados no son validos.", errors));
      }

      GetTaskViewModel newTask = _taskRepository.CreateTask(id, task);

      return Created("api/Usuario", new ApiResponse<GetTaskViewModel>(true, "Tarea creada con éxito.", newTask));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al crear tarea.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  [HttpPut("{id}")]
  public IActionResult UpdateTask(int id, UpdateTaskViewModel task)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      if (!ModelState.IsValid)
      {
        List<string> errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

        return BadRequest(new ApiResponse<List<string>>(false, "Los datos enviados no son validos.", errors));
      }

      _taskRepository.UpdateTask(id, task);

      return Ok(new ApiResponse<string>(true, "Tarea modificada con éxito.", null));
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al actualizar tarea.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al actualizar tarea.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  [HttpPut("assign/{taskId}")]
  public IActionResult AssignTask(int taskId, UpdateAssignTaskViewModel task)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      _taskRepository.AssignTask(task.AssignedUserId, taskId);

      return Ok(new ApiResponse<string>(true, "Tarea asignada con éxito.", null));
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al actualizar tarea.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al actualizar tarea.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  [HttpDelete("{taskId}")]
  public IActionResult delete(int taskId)
  {
    try
    {
      var sessionValidation = ValidateSession();
      if (sessionValidation != null) return sessionValidation;

      _taskRepository.DeleteTask(taskId);

      return NoContent();
    }
    catch (KeyNotFoundException ex)
    {
      _logger.LogWarning(ex.ToString(), "Al eliminar usuario.");
      return BadRequest(new { success = false, message = ex });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString(), "Error al eliminar tarea.");
      return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error interno en el servidor.", ex.Message));
    }
  }

  private IActionResult ValidateSession()
  {
    if (string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
      return Unauthorized(new ApiResponse<string>(false, "No has iniciado sesión.", null));

    return null;
  }
}
