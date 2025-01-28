using Kanban.Models;
using Kanban.ViewModels;
using Kanban.DTO;

namespace Kanban.Interfaces;

public interface IUsuarioRepository
{
  public Usuario CreateUsuario(CreateUsuarioViewModel usuario);
  public void UpdateUsuario(int Id, UpdateUsuarioViewModel usuario);
  public List<Usuario> ObtenerUsuarios();
  public Usuario ObtenerUsuarioId(int id);
  public Usuario ObtenerUsuarioNombre(string nombre);
  public void EliminarUsuario(int id);
  public void CambiarPassword(int id, Usuario usuario);
}
