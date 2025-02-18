using Kanban.Enums;
using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;

public class CreateTareaViewModel
{
  [Required]
  [StringLength(100, ErrorMessage = "El nombre de tarea no debe exceder los 100 caracteres.")]
  public string Nombre { get; set; }
  [Required]
  [Range(1, 5, ErrorMessage = "El estado debe ser 1 (Ideas), 2 (Todo), 3 (Doing), 4 (Review), 5 (Done)")]
  public EstadoTarea Estado { get; set; }
  [Required]
  [StringLength(255, ErrorMessage = "La descripcion no debe exceder los 255 caracteres.")]
  public string Descripcion { get; set; }
  [StringLength(7, ErrorMessage = "El color debe estar en formato hexadecimal")]
  public string? Color { get; set; }
}
