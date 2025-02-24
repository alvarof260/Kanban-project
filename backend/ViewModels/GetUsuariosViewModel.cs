using Kanban.Enums;

namespace Kanban.ViewModels;

public class GetUsuariosViewModel
{
  public int Id { get; set; }
  public string NombreDeUsuario { get; set; }
  public RolUsuario RolUsuario { get; set; }
}
