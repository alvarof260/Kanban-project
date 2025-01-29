using Kanban.Models;
using Kanban.ViewModels;

namespace Kanban.Interfaces;

public interface IUsuarioRepository
{
  public Usuario CreateUsuario(CreateUsuarioViewModel usuario);
  public void UpdateUsuario(int id, UpdateUsuarioViewModel usuario);
  public List<GetUsuariosViewModel> GetUsuarios();
  public Usuario GetUsuarioId(int id);
  public Usuario GetUsuarioNombre(string nombre);
  public void DeleteUsuario(int id);
  public void ChangePassword(int id, Usuario usuario);
}
