using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;

public class LoginViewModel
{
  [Required]
  [StringLength(100, ErrorMessage = "El nombre de usuario no debe exceder los 100 caracteres.")]
  public string NombreDeUsuario { get; set; }
  [Required]
  [StringLength(100, ErrorMessage = "La contraseña no debe superar los 100 caracteres")]
  public string Password { get; set; }
}
