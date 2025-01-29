using Kanban.Models;
using Kanban.ViewModels;

namespace Kanban.Interfaces;

public interface ITableroRepository
{
  public Tablero CreateTablero(CreateTableroViewModel tablero);
  public void UpdateTablero(int id, UpdateTableroViewModel tablero);
  public Tablero GetTableroId(int id);
  public List<GetTablerosViewModel> GetTableros();
  public List<GetTablerosViewModel> GetTablerosIdUsuario(int idUsuario);
  public void DeleteTablero(int id);
}
