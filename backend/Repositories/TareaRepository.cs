using Kanban.Models;
using Kanban.DTO;
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

  public Tarea CrearTarea(int id, Tarea tarea)
  {
    string query = @"
      INSERT INTO Tarea 
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
      command.Parameters.AddWithValue("@IdUsuarioAsignado", tarea.IdUsuarioAsignado);

      int idGenerado = Convert.ToInt32(command.ExecuteScalar());
      tarea.Id = idGenerado;
      tarea.IdTablero = id;

      connection.Close();
    }
    return tarea;
  }

  public void ModificarTarea(int id, TareaDTO tarea)
  {
    string query = @"
      UPDATE Tarea
      SET 
          nombre = COALESCE(@Nombre, nombre), 
          estado = COALESCE(@Estado, estado), 
          descripcion = COALESCE(@Descripcion, descripcion), 
          color = COALESCE(@Color, color)
      WHERE id = @Id;
      ";
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

      command.ExecuteNonQuery();

      connection.Close();
    }
  }

  public Tarea ObtenerDetalle(int id)
  {
    Tarea tareaBuscado = new Tarea();
    string query = @"SELECT * FROM Tarea WHERE id = @Id;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);
      using (SqliteDataReader reader = command.ExecuteReader())
      {
        tareaBuscado.Id = reader.GetInt32(0);
        tareaBuscado.IdTablero = reader.GetInt32(1);
        tareaBuscado.Nombre = reader.GetString(2);
        tareaBuscado.Estado = (EstadoTarea)reader.GetInt32(3);
        tareaBuscado.Descripcion = reader.GetString(4);
        tareaBuscado.Color = reader.GetString(5);
        tareaBuscado.IdUsuarioAsignado = reader.GetInt32(6);
      }

      connection.Close();
    }
    return tareaBuscado;
  }

  public List<Tarea> ObtenerTareasPorUsuario(int id)
  {
    List<Tarea> tareas = new List<Tarea>();
    string query = @"SELECT * FROM Tarea WHERE id_usuario_asignado = @Id;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@Id", id);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tareas.Add(new Tarea
          {
            Id = reader.GetInt32(0),
            IdTablero = reader.GetInt32(1),
            Nombre = reader.GetString(2),
            Estado = (EstadoTarea)reader.GetInt32(3),
            Descripcion = reader.GetString(4),
            Color = reader.GetString(5),
            IdUsuarioAsignado = reader.GetInt32(6)
          });
        }
      }
      connection.Close();
    }
    return tareas;
  }

  public List<Tarea> ObtenerTareaPorTablero(int id)
  {
    List<Tarea> tareas = new List<Tarea>();
    string query = @"SELECT * FROM Tarea WHERE id_tablero = @Id;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@Id", id);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tareas.Add(new Tarea
          {
            Id = reader.GetInt32(0),
            IdTablero = reader.GetInt32(1),
            Nombre = reader.GetString(2),
            Estado = (EstadoTarea)reader.GetInt32(3),
            Descripcion = reader.GetString(4),
            Color = reader.GetString(5),
            IdUsuarioAsignado = reader.GetInt32(6)
          });
        }
      }
      connection.Close();
    }
    return tareas;
  }

  public void EliminarTarea(int id)
  {
    string query = @"DELETE FROM Tarea WHERE id = @Id;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);

      command.ExecuteNonQuery();

      connection.Close();
    }
  }

  public void AsignarTarea(int idUsuario, int idTarea)
  {
    string query = @"UPDATE Tarea SET id_usuario_asignado = @IdUsuarioAsignado WHERE id = @Id;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@IdUsuarioAsignado", idUsuario);
      command.Parameters.AddWithValue("@Id", idTarea);

      command.ExecuteNonQuery();

      connection.Close();
    }
  }
}
