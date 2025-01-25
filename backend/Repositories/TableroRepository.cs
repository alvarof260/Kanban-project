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

  public Tablero ObtenerDetalles(int id)
  {
    Tablero tableroBuscado = new Tablero();
    string query = @"SELECT id_usuario_propietario, nombre, descripcion FROM Tablero WHERE id = @Id";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tableroBuscado.Id = reader.GetInt32(0);
          tableroBuscado.IdUsuarioPropietario = reader.GetInt32(1);
          tableroBuscado.Nombre = reader.GetString(2);
          tableroBuscado.Descripcion = reader.GetString(3);
        }
      }

      connection.Close();
    }
    return tableroBuscado;
  }

  public List<Tablero> ObtenerTableros()
  {
    List<Tablero> tableros = new List<Tablero>();
    string query = @"SELECT id_usuario_propietario, nombre, descripcion FROM Tablero;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tableros.Add(new Tablero
          {
            IdUsuarioPropietario = reader.GetInt32(0),
            Nombre = reader.GetString(1),
            Descripcion = reader.GetString(2)
          });
        }
      }

      connection.Close();
    }
    return tableros;
  }

}
