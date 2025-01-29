using Kanban.Enums;

namespace Kanban.ViewModels;

public class GetTareasViewModel
{
  public int IdTablero { get; set; }
  public string Nombre { get; set; }
  public EstadoTarea Estado { get; set; }
  public string Descripcion { get; set; }
  public string Color { get; set; }
  public int? IdUsuarioAsignado { get; set; }
}
