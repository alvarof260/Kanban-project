using Kanban.Models;
using Kanban.ViewModels;

namespace Kanban.Interfaces;

public interface ITaskRepository
{
  public GetTaskViewModel CreateTask(int boardId, CreateTaskViewModel task);
  public void UpdateTask(int id, UpdateTaskViewModel task);
  public TaskBoard GetTaskById(int id);
  public List<GetTaskViewModel> GetTasksByUserId(int userId);
  public List<GetTaskViewModel> GetTasksByBoardId(int boardId);
  public void DeleteTask(int id);
  public void AssignTask(int userId, int taskId);
}
