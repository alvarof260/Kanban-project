using Kanban.Models;
using Kanban.DTO;

namespace Kanban.Interfaces;

public interface IUsuarioRepository
{
  public Usuario CrearUsuario(Usuario usuario);
  public void ModificarUsuario(int Id, UsuarioDTO usuario);
  public List<Usuario> ObtenerUsuarios();
  public Usuario ObtenerUsuarioId(int id);
  public Usuario ObtenerUsuarioNombre(string nombre);
  public void EliminarUsuario(int id);
  public void CambiarPassword(int id, Usuario usuario);
}
