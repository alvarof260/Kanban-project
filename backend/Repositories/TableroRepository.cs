using Microsoft.Data.Sqlite;
using Kanban.Models;

namespace Kanban.Repositories;

public interface ITableroRepository
{
  public Tablero CrearTablero(Tablero tablero);
  public void ModificarTablero(int id, Tablero tablero);
  public Tablero ObtenerDetalles(int id);
  public List<Tablero> ObtenerTableros();
  public List<Tablero> ObtenerTablerosId();
  public void EliminarTablero(int id);
}

public class TableroRepository : ITableroRepository
{
  private readonly string _connectionString;

  public TableroRepository(string connectionString)
  {
    this._connectionString = connectionString;
  }

  public Tablero CrearTablero(Tablero tablero)
  {
    string query = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) VALUES (@IdUsuarioPropietario, @Nombre, @Descripcion);";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@IdUsuarioPropietario", tablero.IdUsuarioPropietario);
      command.Parameters.AddWithValue("@Nombre", tablero.Nombre);
      command.Parameters.AddWithValue("@Descripcion", tablero.Descripcion);

      command.ExecuteNonQuery();

      connection.Close();
    }
  }

  public void ModificarTablero(int id, Tablero tablero)
  {
    string query = @"UPDATE Tablero SET nombre = @Nombre, descripcion = @Descripcion WHERE id = @Id;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Nombre", tablero.Nombre);
      command.Parameters.AddWithValue("@Descripcion", tablero.Descripcion);
      command.Parameters.AddWithValue("@Id", id);

      command.ExecuteNonQuery();

      connection.Close();
    }
  }
}
