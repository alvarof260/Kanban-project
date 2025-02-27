using Kanban.Enums;
using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;

public class CreateUserViewModel
{
  [Required]
  [StringLength(100, ErrorMessage = "El nombre de usuario no debe exceder los 100 caracteres.")]
  public string Username { get; set; }
  [Required]
  [StringLength(100, ErrorMessage = "La contraseña no debe superar los 100 caracteres")]
  public string Password { get; set; }
  [Required]
  [Range(1, 2, ErrorMessage = "El rol de usuario debe ser 1 (Administrador) o 2 (Operador)")]
  public RoleUser RoleUser { get; set; }
}
