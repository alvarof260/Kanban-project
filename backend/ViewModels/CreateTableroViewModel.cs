using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;

public class CreateTableroViewModel
{
  [Required]
  public int IdUsuarioPropietario { get; set; }
  [Required]
  [StringLength(100, ErrorMessage = "El nombre de tablero no debe exceder los 100 caracteres.")]
  public string Nombre { get; set; }
  [Required]
  [StringLength(255, ErrorMessage = "La descripcion no debe exceder los 255 caracteres.")]
  public string Descripcion { get; set; }
}

