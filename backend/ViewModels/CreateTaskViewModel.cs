using Kanban.Enums;
using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;

public class CreateTaskViewModel
{
  [Required]
  [StringLength(100, ErrorMessage = "El nombre de tarea no debe exceder los 100 caracteres.")]
  public string Name { get; set; }
  [Required]
  [Range(1, 5, ErrorMessage = "El estado debe ser 1 (Ideas), 2 (Todo), 3 (Doing), 4 (Review), 5 (Done)")]
  public StatusTask Status { get; set; }
  [Required]
  [StringLength(255, ErrorMessage = "La descripcion no debe exceder los 255 caracteres.")]
  public string Description { get; set; }
  [StringLength(30, ErrorMessage = "El color no debe exceder los 30 caracteres.")]
  public string? Color { get; set; }
}
