using Kanban.Models;
using Kanban.ViewModels;

namespace Kanban.Interfaces;

public interface ITareaRepository
{
  public Tarea CreateTarea(int id, CreateTareaViewModel tarea);
  public void UpdateTarea(int id, UpdateTareaViewModel tarea);
  public Tarea GetTareaById(int id);
  public List<GetTareasViewModel> GetTareaByIdUsuario(int idUsuario);
  public List<GetTareasViewModel> GetTareaByIdTablero(int idTablero);
  public void DeleteTarea(int id);
  public void AssignTarea(int idUsuario, int idTarea);
}
