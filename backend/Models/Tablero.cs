
namespace Kanban.Models;

public class Tablero
{
  private int _id;
  private int _idUsuarioPropietario;
  private string _nombre;
  private string _descripcion;

  public int Id { get => _id; set => _id = value; }
  public int IdUsuarioPropietario { get => _idUsuarioPropietario; set => _idUsuarioPropietario = value; }
  public string Nombre { get => _nombre; set => _nombre = value; }
  public string Descripcion { get => _descripcion; set => _descripcion = value; }

  public Tablero(int id, int idUsuarioPropietario, string nombre, string descripcion)
  {
    this.Id = id;
    this.IdUsuarioPropietario = idUsuarioPropietario;
    this.Nombre = nombre;
    this.Descripcion = descripcion;
  }

  public Tablero() { }
}
