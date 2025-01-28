using Kanban.Models;
using Kanban.DTO;

namespace Kanban.Interfaces;

public interface ITareaRepository
{
  public Tarea CrearTarea(int id, Tarea tarea);
  public void ModificarTarea(int id, TareaDTO tarea);
  public Tarea ObtenerDetalle(int id);
  public List<Tarea> ObtenerTareasPorUsuario(int id);
  public List<Tarea> ObtenerTareaPorTablero(int id);
  public void EliminarTarea(int id);
  public void AsignarTarea(int idUsuario, int idTarea);
}
