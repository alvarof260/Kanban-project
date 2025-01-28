using Kanban.Enums;

namespace Kanban.Models
{
  public class Usuario
  {
    private int _id;
    private string _nombreDeUsuario;
    private string _password;
    private RolUsuario _rolUsuario;


    public int Id { get => _id; set => _id = value; }
    public string NombreDeUsuario { get => _nombreDeUsuario; set => _nombreDeUsuario = value; }
    public string Password { get => _password; set => _password = value; }
    public RolUsuario RolUsuario { get => _rolUsuario; set => _rolUsuario = value; }

    public Usuario(int id, string nombreDeUsuario, string password, RolUsuario rolUsuario)
    {
      this.Id = id;
      this.NombreDeUsuario = nombreDeUsuario;
      this.Password = password;
      this.RolUsuario = rolUsuario;
    }

    public Usuario() { }
  }
}
