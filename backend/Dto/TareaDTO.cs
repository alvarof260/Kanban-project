using Kanban.Enums;

namespace Kanban.DTO;

public class TareaDTO
{
  public string? Nombre { get; set; }
  public EstadoTarea? Estado { get; set; }
  public string? Descripcion { get; set; }
  public string? Color { get; set; }
  public int? IdUsuarioAsignado { get; set; }
}
