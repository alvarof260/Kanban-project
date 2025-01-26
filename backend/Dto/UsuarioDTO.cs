using Kanban.Enums;

namespace Kanban.DTO;

public class UsuarioDTO
{
  public string? NombreDeUsuario { get; set; }
  public string? Password { get; set; }
  public RolUsuario? RolUsuario { get; set; }
}
