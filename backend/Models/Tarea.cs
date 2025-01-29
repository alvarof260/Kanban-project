using Kanban.Enums;

namespace Kanban.Models
{
  public class Tarea
  {
    private int _id;
    private int _idTablero;
    private string _nombre;
    private string _descripcion;
    private string _color;
    private EstadoTarea _estado;
    private int? _idUsuarioAsignado;

    public int Id { get => _id; set => _id = value; }
    public int IdTablero { get => _idTablero; set => _idTablero = value; }
    public string Nombre { get => _nombre; set => _nombre = value; }
    public string Descripcion { get => _descripcion; set => _descripcion = value; }
    public string Color { get => _color; set => _color = value; }
    public EstadoTarea Estado { get => _estado; set => _estado = value; }
    public int? IdUsuarioAsignado { get => _idUsuarioAsignado; set => _idUsuarioAsignado = value; }

    public Tarea(int id, int idTablero, string nombre, string descripcion, string color, EstadoTarea estado, int idUsuarioAsignado)
    {
      this.Id = id;
      this.IdTablero = idTablero;
      this.Nombre = nombre;
      this.Descripcion = descripcion;
      this.Color = color;
      this.Estado = estado;
      this.IdUsuarioAsignado = idUsuarioAsignado;
    }

    public Tarea() { }
  }
}
