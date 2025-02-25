using Kanban.Models;
using Kanban.ViewModels;
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

  public Tablero CreateTablero(CreateTableroViewModel tablero)
  {
    Tablero? nuevoTablero = null;

    string query = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) 
                     VALUES (@IdUsuarioPropietario, @Nombre, @Descripcion);
                     SELECT last_insert_rowid();";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@IdUsuarioPropietario", tablero.IdUsuarioPropietario);
      command.Parameters.AddWithValue("@Nombre", tablero.Nombre);
      command.Parameters.AddWithValue("@Descripcion", tablero.Descripcion);

      int idGenerado = Convert.ToInt32(command.ExecuteScalar());

      nuevoTablero = new Tablero
      {
        Id = idGenerado,
        IdUsuarioPropietario = tablero.IdUsuarioPropietario,
        Nombre = tablero.Nombre,
        Descripcion = tablero.Descripcion
      };

      connection.Close();
    }

    return nuevoTablero;
  }

  public void UpdateTablero(int id, UpdateTableroViewModel tablero)
  {
    string query = @"UPDATE Tablero 
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

      int tableroUpdate = command.ExecuteNonQuery();

      if (tableroUpdate == 0)
      {
        throw new KeyNotFoundException($"No se encontro el tablero con ID: {id}.");
      }

      connection.Close();
    }
  }

  public int GetIdPropietario(int id)
  {
    string query = @"SELECT id_usuario_propietario 
                     FROM Tablero 
                     WHERE id = @Id";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      using (SqliteCommand command = new SqliteCommand(query, connection))
      {
        command.Parameters.AddWithValue("@Id", id);
        object result = command.ExecuteScalar();

        if (result == null)
        {
          throw new KeyNotFoundException($"No se encontró el tablero con ID {id}.");
        }
        return Convert.ToInt32(result);
      }
    }
  }

  public GetTablerosViewModel GetTableroId(int id)
  {
    GetTablerosViewModel tableroBuscado = null;

    string query = @"SELECT t.id, u.nombre_de_usuario, t.nombre, t.descripcion 
                     FROM Tablero t 
                     LEFT JOIN Usuario u ON t.id_usuario_propietario = u.id
                     WHERE t.id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        if (reader.Read())
        {
          tableroBuscado = new GetTablerosViewModel
          {
            Id = reader.GetInt32(0),
            NombreUsuarioPropietario = reader.GetString(1),
            Nombre = reader.GetString(2),
            Descripcion = reader.GetString(3)
          };
        }
      }

      connection.Close();
    }

    if (tableroBuscado == null)
    {
      throw new KeyNotFoundException($"No se encontro el tablero con ID: {id}.");
    }

    return tableroBuscado;
  }

  public List<GetTablerosViewModel> GetTableros()
  {
    List<GetTablerosViewModel> tableros = new List<GetTablerosViewModel>();

    string query = @"SELECT t.id, u.nombre_de_usuario , t.nombre, t.descripcion 
                     FROM Tablero t
                     LEFT JOIN Usuario u ON t.id_usuario_propietario = u.id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tableros.Add(new GetTablerosViewModel
          {
            Id = reader.GetInt32(0),
            NombreUsuarioPropietario = reader.GetString(1),
            Nombre = reader.GetString(2),
            Descripcion = reader.GetString(3)
          });
        }
      }

      connection.Close();
    }

    return tableros;
  }

  public List<GetTablerosViewModel> GetTablerosIdUsuario(int idUsuario)
  {
    List<GetTablerosViewModel> tablerosBuscado = new List<GetTablerosViewModel>();

    string query = @"SELECT t.id, 
       u_p.nombre_de_usuario AS nombre_de_usuario,
       t.nombre, 
       t.descripcion
FROM Tablero t                                                       
LEFT JOIN Tarea ta ON t.id = ta.id_tablero                         
LEFT JOIN Usuario u_t ON ta.id_usuario_asignado = u_t.id
LEFT JOIN Usuario u_p ON t.id_usuario_propietario = u_p.id                
WHERE t.id_usuario_propietario = @Id OR ta.id_usuario_asignado = @Id
GROUP BY t.id, u_p.nombre_de_usuario, t.nombre, t.descripcion;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", idUsuario);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tablerosBuscado.Add(new GetTablerosViewModel
          {
            Id = reader.GetInt32(0),
            NombreUsuarioPropietario = reader.GetString(1),
            Nombre = reader.GetString(2),
            Descripcion = reader.GetString(3)
          });
        }
      }

      connection.Close();
    }

    return tablerosBuscado;
  }

  public void DeleteTablero(int id)
  {
    if (TieneTarea(id))
    {
      throw new InvalidOperationException("No se puede eliminar el tablero porque tiene tareas asignadas.");
    }

    string query = @"DELETE FROM tablero
                     WHERE id = @Id";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);

      int filasAfectadas = command.ExecuteNonQuery();

      if (filasAfectadas == 0)
      {
        throw new KeyNotFoundException($"No se encontro el tablero con ID: {id}.");
      }

      connection.Close();
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
