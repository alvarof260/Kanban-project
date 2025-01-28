using Kanban.Models;
using Kanban.DTO;

namespace Kanban.Interfaces;

public interface ITableroRepository
{
  public Tablero CrearTablero(Tablero tablero);
  public void ModificarTablero(int id, TableroDTO tablero);
  public Tablero ObtenerDetalles(int id);
  public List<Tablero> ObtenerTableros();
  public List<Tablero> ObtenerTablerosId(int id);
  public void EliminarTablero(int id);
}
