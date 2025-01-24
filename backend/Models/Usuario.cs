using System.ComponentModel.DataAnnotations;
using Kanban.Enums;

namespace Kanban.Models
{
  public class Usuario
  {
    // campos privados.
    private int _id;
    private string _nombreDeUsuario;
    private string _password;
    private RolUsuario _rolUsuario;


    [Key]
    public int Id { get => _id; set => _id = value; }

    [Required]
    [StringLength(100)]
    public string NombreDeUsuario { get => _nombreDeUsuario; set => _nombreDeUsuario = value; }

    [Required]
    [StringLength(100)]
    public string Password { get => _password; set => _password = value; }

    [Required]
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
