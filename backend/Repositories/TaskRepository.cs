using Kanban.Models;
using Kanban.ViewModels;
using Kanban.Enums;
using Kanban.Interfaces;
using Microsoft.Data.Sqlite;

namespace Kanban.Repositories;

public class TaskRepository : ITaskRepository
{
  private readonly string _connectionString;

  public TaskRepository(string connectionString)
  {
    this._connectionString = connectionString;
  }

  public GetTaskViewModel CreateTask(int boardId, CreateTaskViewModel task)
  {
    GetTaskViewModel newTask = null;

    string query = @"INSERT INTO Tarea 
                     (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado)
                     VALUES (@IdTablero, @Nombre, @Estado, @Descripcion, @Color, @IdUsuarioAsignado);
                     SELECT last_insert_rowid();";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@IdTablero", boardId);
      command.Parameters.AddWithValue("@Nombre", task.Name);
      command.Parameters.AddWithValue("@Estado", task.Status);
      command.Parameters.AddWithValue("@Descripcion", task.Description);
      command.Parameters.AddWithValue("@Color", task.Color);
      command.Parameters.AddWithValue("@IdUsuarioAsignado", 0);

      int id = Convert.ToInt32(command.ExecuteScalar());

      newTask = new GetTaskViewModel
      {
        Id = id,
        Name = task.Name,
        Status = task.Status,
        Description = task.Description,
        Color = task.Color,
        AssignedUserId = 0,
        AssignedUserName = ""
      };

      connection.Close();
    }
    return newTask;
  }

  public void UpdateTask(int id, UpdateTaskViewModel task)
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
         string.IsNullOrEmpty(task.Name) ? DBNull.Value : task.Name);
      command.Parameters.AddWithValue("@Estado",
          task.Status.HasValue ? task.Status : DBNull.Value);
      command.Parameters.AddWithValue("@Descripcion",
          string.IsNullOrEmpty(task.Description) ? DBNull.Value : task.Description);
      command.Parameters.AddWithValue("@Color",
          string.IsNullOrEmpty(task.Color) ? DBNull.Value : task.Color);
      command.Parameters.AddWithValue("@Id", id);

      int rowsAffected = command.ExecuteNonQuery();

      if (rowsAffected == 0)
      {
        throw new KeyNotFoundException($"No se encontro la tarea con ID: {id}.");
      }

      connection.Close();
    }
  }

  public TaskBoard GetTaskById(int id)
  {
    TaskBoard taskFound = null;

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
          taskFound = new TaskBoard
          {
            Id = reader.GetInt32(0),
            BoardId = reader.GetInt32(1),
            Name = reader.GetString(2),
            Status = (StatusTask)reader.GetInt32(3),
            Description = reader.GetString(4),
            Color = reader.GetString(5),
            AssignedUserId = reader.GetInt32(6)
          };
        }
      }

      connection.Close();
    }

    if (taskFound == null)
    {
      throw new KeyNotFoundException($"No se encontro la tarea con ID: {id}.");
    }

    return taskFound;
  }

  public List<GetTaskViewModel> GetTasksByUserId(int userId)
  {
    List<GetTaskViewModel> tasks = new List<GetTaskViewModel>();

    string query = @"SELECT 
                      t.id, 
                      t.nombre, 
                      t.estado, 
                      t.descripcion, 
                      t.color, 
                      t.id_usuario_asignado, 
                      u.nombre_de_usuario
                     FROM Tarea t
                     LEFT JOIN Usuario u ON t.id_usuario_asignado = u.id
                     WHERE id_usuario_asignado = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@Id", userId);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tasks.Add(new GetTaskViewModel
          {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Status = (StatusTask)reader.GetInt32(2),
            Description = reader.GetString(3),
            Color = reader.GetString(4),
            AssignedUserId = reader.GetInt32(5),
            AssignedUserName = reader.IsDBNull(6) ? "" : reader.GetString(6)
          });
        }
      }
      connection.Close();
    }

    return tasks;
  }

  public List<GetTaskViewModel> GetTasksByBoardId(int boardId)
  {
    List<GetTaskViewModel> tasks = new List<GetTaskViewModel>();

    string query = @"SELECT 
                      t.id, 
                      t.nombre, 
                      t.estado, 
                      t.descripcion, 
                      t.color, 
                      t.id_usuario_asignado, 
                      COALESCE(u.nombre_de_usuario, '') AS nombre_de_usuario_asignado
                     FROM Tarea t
                     LEFT JOIN Usuario u ON t.id_usuario_asignado = u.id
                     WHERE id_tablero = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@Id", boardId);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          tasks.Add(new GetTaskViewModel
          {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Status = (StatusTask)reader.GetInt32(2),
            Description = reader.GetString(3),
            Color = reader.GetString(4),
            AssignedUserId = reader.GetInt32(5),
            AssignedUserName = reader.IsDBNull(6) ? "" : reader.GetString(6)
          });
        }
      }
      connection.Close();
    }

    return tasks;
  }

  public void DeleteTask(int id)
  {
    string query = @"DELETE FROM Tarea 
                     WHERE id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);

      int rowsAffected = command.ExecuteNonQuery();

      if (rowsAffected == 0)
      {
        throw new KeyNotFoundException($"No se encontro la tarea con ID: {id}.");
      }

      connection.Close();
    }
  }

  public void AssignTask(int userId, int taskId)
  {
    string query = @"UPDATE Tarea 
                     SET id_usuario_asignado = @IdUsuarioAsignado 
                     WHERE id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@IdUsuarioAsignado", userId);
      command.Parameters.AddWithValue("@Id", taskId);

      int rowsAffected = command.ExecuteNonQuery();

      if (rowsAffected == 0)
      {
        throw new KeyNotFoundException($"No se encontro la tarea con ID: {taskId}.");
      }

      connection.Close();
    }
  }
}
