using Kanban.Models;
using Kanban.DTO;
using Kanban.Interfaces;
using Microsoft.Data.Sqlite;

namespace Kanban.Repositories;

public class TableroRepository : ITableroRepository
{
  private readonly string _connectionString;

  public TableroRepository(string connectionString)
  {
    this._connectionString = connectionString;
  }

  public Tablero CrearTablero(Tablero tablero)
  {
    string query = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) VALUES (@IdUsuarioPropietario, @Nombre, @Descripcion);
                     SELECT last_insert_rowid();";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@IdUsuarioPropietario", tablero.IdUsuarioPropietario);
      command.Parameters.AddWithValue("@Nombre", tablero.Nombre);
      command.Parameters.AddWithValue("@Descripcion", tablero.Descripcion);

      int idGenerado = Convert.ToInt32(command.ExecuteScalar());
      tablero.Id = idGenerado;

      connection.Close();
    }
    return tablero;
  }

  public void ModificarTablero(int id, TableroDTO tablero)
  {
    string query = @"
                   UPDATE Tablero 
                   SET 
                      nombre = COALESCE(@Nombre, nombre), 
                      descripcion = COALESCE(@Descripcion, descripcion) 
                   WHERE id = @Id;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@Id", id);

      command.Parameters.AddWithValue("@Nombre",
          string.IsNullOrEmpty(tablero.Nombre) ? DBNull.Value : tablero.Nombre);
      command.Parameters.AddWithValue("@Descripcion",
          string.IsNullOrEmpty(tablero.Descripcion) ? DBNull.Value : tablero.Descripcion);

      command.ExecuteNonQuery();

      connection.Close();
    }
  }

  public Tablero ObtenerDetalles(int id)
  {
    Tablero tableroBuscado = new Tablero();
    string query = @"SELECT * FROM Tablero WHERE id = @Id";
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
    string query = @"SELECT * FROM Tablero;";
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
            Id = reader.GetInt32(0),
            IdUsuarioPropietario = reader.GetInt32(1),
            Nombre = reader.GetString(2),
            Descripcion = reader.GetString(3)
          });
        }
      }

      connection.Close();
    }
    return tableros;
  }

  public List<Tablero> ObtenerTablerosId(int id)
  {
    List<Tablero> tablerosBuscado = new List<Tablero>();
    string query = @"";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tablerosBuscado.Add(new Tablero
          {
            IdUsuarioPropietario = reader.GetInt32(0),
            Nombre = reader.GetString(1),
            Descripcion = reader.GetString(2)
          });
        }
      }

      connection.Close();
    }
    return tablerosBuscado;
  }

  public void EliminarTablero(int id)
  {
    string query = @"DELETE FROM tablero WHERE id = @Id";
    if (!TieneTarea(id))
    {
      using (SqliteConnection connection = new SqliteConnection(_connectionString))
      {
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();

        connection.Close();
      }
    }
  }

  private bool TieneTarea(int id)
  {
    int contador;
    string query = @"SELECT COUNT(*) FROM Tarea WHERE id_tablero = @Id;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);
      contador = Convert.ToInt32(command.ExecuteScalar());

      connection.Close();
    }
    return contador > 0;
  }
}
