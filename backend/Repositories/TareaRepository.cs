using Kanban.Models;
using Kanban.ViewModels;
using Kanban.Enums;
using Kanban.Interfaces;
using Microsoft.Data.Sqlite;

namespace Kanban.Repositories;

public class TareaRepository : ITareaRepository
{
  private readonly string _connectionString;

  public TareaRepository(string connectionString)
  {
    this._connectionString = connectionString;
  }

  public Tarea CreateTarea(int id, CreateTareaViewModel tarea)
  {
    Tarea nuevaTarea = null;

    string query = @"INSERT INTO Tarea 
                     (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado)
                     VALUES (@IdTablero, @Nombre, @Estado, @Descripcion, @Color, @IdUsuarioAsignado);
                     SELECT last_insert_rowid();";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@IdTablero", id);
      command.Parameters.AddWithValue("@Nombre", tarea.Nombre);
      command.Parameters.AddWithValue("@Estado", tarea.Estado);
      command.Parameters.AddWithValue("@Descripcion", tarea.Descripcion);
      command.Parameters.AddWithValue("@Color", tarea.Color);
      command.Parameters.AddWithValue("@IdUsuarioAsignado", 0);

      int idGenerado = Convert.ToInt32(command.ExecuteScalar());

      nuevaTarea = new Tarea
      {
        Id = idGenerado,
        IdTablero = id,
        Nombre = tarea.Nombre,
        Estado = tarea.Estado,
        Descripcion = tarea.Descripcion,
        Color = tarea.Color,
        IdUsuarioAsignado = 0
      };

      connection.Close();
    }
    return nuevaTarea;
  }

  public void UpdateTarea(int id, UpdateTareaViewModel tarea)
  {
    string query = @"UPDATE Tarea
                     SET 
                     nombre = COALESCE(@Nombre, nombre), 
                     estado = COALESCE(@Estado, estado), 
                     descripcion = COALESCE(@Descripcion, descripcion), 
                     color = COALESCE(@Color, color)
                     WHERE id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Nombre",
         string.IsNullOrEmpty(tarea.Nombre) ? DBNull.Value : tarea.Nombre);
      command.Parameters.AddWithValue("@Estado",
          tarea.Estado.HasValue ? tarea.Estado : DBNull.Value);
      command.Parameters.AddWithValue("@Descripcion",
          string.IsNullOrEmpty(tarea.Descripcion) ? DBNull.Value : tarea.Descripcion);
      command.Parameters.AddWithValue("@Color",
          string.IsNullOrEmpty(tarea.Color) ? DBNull.Value : tarea.Color);
      command.Parameters.AddWithValue("@Id", id);

      int filasAfectadas = command.ExecuteNonQuery();

      if (filasAfectadas == 0)
      {
        throw new KeyNotFoundException($"No se encontro la tarea con ID: {id}.");
      }

      connection.Close();
    }
  }

  public Tarea GetTareaById(int id)
  {
    Tarea tareaBuscado = null;

    string query = @"SELECT * 
                     FROM Tarea 
                     WHERE id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);
      using (SqliteDataReader reader = command.ExecuteReader())
      {
        if (reader.Read())
        {
          tareaBuscado = new Tarea
          {
            Id = reader.GetInt32(0),
            IdTablero = reader.GetInt32(1),
            Nombre = reader.GetString(2),
            Estado = (EstadoTarea)reader.GetInt32(3),
            Descripcion = reader.GetString(4),
            Color = reader.GetString(5),
            IdUsuarioAsignado = reader.GetInt32(6)
          };
        }
      }

      connection.Close();
    }

    if (tareaBuscado == null)
    {
      throw new KeyNotFoundException($"No se encontro la tarea con ID: {id}.");
    }

    return tareaBuscado;
  }

  public List<GetTareasViewModel> GetTareaByIdUsuario(int idUsuario)
  {
    List<GetTareasViewModel> tareas = new List<GetTareasViewModel>();

    string query = @"SELECT id, nombre, estado, descripcion, color, id_usuario_asignado, u.nombre_de_usuario
                     FROM Tarea 
                     LEFT JOIN Usuario u ON t.id_usuario_asignado = u.id
                     WHERE id_usuario_asignado = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@Id", idUsuario);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tareas.Add(new GetTareasViewModel
          {
            Id = reader.GetInt32(0),
            Nombre = reader.GetString(1),
            Estado = (EstadoTarea)reader.GetInt32(2),
            Descripcion = reader.GetString(3),
            Color = reader.GetString(4),
            IdUsuarioAsignado = reader.GetInt32(5),
            NombreUsuarioAsignado = reader.IsDBNull(6) ? null : reader.GetString(6)
          });
        }
      }
      connection.Close();
    }

    return tareas;
  }

  public List<GetTareasViewModel> GetTareaByIdTablero(int idTablero)
  {
    List<GetTareasViewModel> tareas = new List<GetTareasViewModel>();

    string query = @"SELECT 
                      id, 
                      nombre, 
                      estado, 
                      descripcion, 
                      color, 
                      id_usuario_asignado, 
                      COALESCE(u.nombre_de_usuario, '') AS nombre_de_usuario_asignado
                     FROM Tarea t
                     LEFT JOIN Usuario u ON t.id_usuario_asignado = u.id
                     WHERE id_tablero = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@Id", idTablero);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tareas.Add(new GetTareasViewModel
          {
            Id = reader.GetInt32(0),
            Nombre = reader.GetString(1),
            Estado = (EstadoTarea)reader.GetInt32(2),
            Descripcion = reader.GetString(3),
            Color = reader.GetString(4),
            IdUsuarioAsignado = reader.GetInt32(5),
            NombreUsuarioAsignado = reader.IsDBNull(6) ? "" : reader.GetString(6)
          });
        }
      }
      connection.Close();
    }

    return tareas;
  }

  public void DeleteTarea(int id)
  {
    string query = @"DELETE FROM Tarea 
                     WHERE id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);

      int filasAfectadas = command.ExecuteNonQuery();

      if (filasAfectadas == 0)
      {
        throw new KeyNotFoundException($"No se encontro la tarea con ID: {id}.");
      }

      connection.Close();
    }
  }

  public void AssignTarea(int idUsuario, int idTarea)
  {
    string query = @"UPDATE Tarea 
                     SET id_usuario_asignado = @IdUsuarioAsignado 
                     WHERE id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@IdUsuarioAsignado", idUsuario);
      command.Parameters.AddWithValue("@Id", idTarea);

      int filasAfectadas = command.ExecuteNonQuery();

      if (filasAfectadas == 0)
      {
        throw new KeyNotFoundException($"No se encontro la tarea con ID: {idTarea}.");
      }

      connection.Close();
    }
  }
}
