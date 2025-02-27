using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;

public class LoginViewModel
{
  [Required]
  [StringLength(50, ErrorMessage = "El nombre de usuario no debe exceder los 50 caracteres.")]
  public string Username { get; set; }
  [Required]
  [StringLength(50, ErrorMessage = "La contraseña no debe superar los 50 caracteres")]
  public string Password { get; set; }
}
