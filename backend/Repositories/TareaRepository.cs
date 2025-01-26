using Kanban.Models;
using Microsoft.Data.Sqlite;

namespace Kanban.Repositories;

public interface ITareaRepository
{
  public Tarea CrearTarea(int id, Tarea tarea);
  public void ModificarTarea(int id, Tarea tarea);
}

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
      VALUES (@IdTablero, @Nombre, @Estado, @Descripcion, @Color, @IdUsuarioAsignado);";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@IdTablero", tarea.IdTablero);
      command.Parameters.AddWithValue("@Nombre", tarea.Nombre);
      command.Parameters.AddWithValue("@Estado", tarea.Estado);
      command.Parameters.AddWithValue("@Descripcion", tarea.Descripcion);
      command.Parameters.AddWithValue("@Color", tarea.Color);
      command.Parameters.AddWithValue("@IdUsuarioAsignado", tarea.IdUsuarioAsignado);

      command.ExecuteNonQuery();

      connection.Close();
    }
    return tarea;
  }

  public void ModificarTarea(int id, Tarea tarea)
  {
    string query = @"
      UPDATE Tarea
      SET nombre = @Nombre, estado = @Estado, descripcion = @Descripcion, color = @Color
      WHERE id = @Id;
      ";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Nombre", tarea.Nombre);
      command.Parameters.AddWithValue("@Estado", tarea.Estado);
      command.Parameters.AddWithValue("@Descripcion", tarea.Descripcion);
      command.Parameters.AddWithValue("@Color", tarea.Color);

      command.ExecuteNonQuery();

      connection.Close();
    }
  }
}
