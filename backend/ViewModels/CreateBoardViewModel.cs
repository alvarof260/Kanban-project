using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;

public class CreateBoardViewModel
{
  [Required]
  public int OwnerUserId { get; set; }
  [Required]
  [StringLength(100, ErrorMessage = "El nombre de tablero no debe exceder los 100 caracteres.")]
  public string Name { get; set; }
  [StringLength(255, ErrorMessage = "La descripcion no debe exceder los 255 caracteres.")]
  public string? Description { get; set; }
}

