using Kanban.Models;
using Kanban.ViewModels;
using Kanban.Interfaces;
using Microsoft.Data.Sqlite;

namespace Kanban.Repositories;

public class BoardRepository : IBoardRepository
{
  private readonly string _connectionString;

  public BoardRepository(string connectionString)
  {
    this._connectionString = connectionString;
  }

  public Board CreateBoard(CreateBoardViewModel board)
  {
    Board newBoard = null;

    string query = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) 
                     VALUES (@IdUsuarioPropietario, @Nombre, @Descripcion);
                     SELECT last_insert_rowid();";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@IdUsuarioPropietario", board.OwnerUserId);
      command.Parameters.AddWithValue("@Nombre", board.Name);
      command.Parameters.AddWithValue("@Descripcion", board.Description);

      int id = Convert.ToInt32(command.ExecuteScalar());

      newBoard = new Board
      {
        Id = id,
        OwnerUserId = board.OwnerUserId,
        Name = board.Name,
        Description = board.Description
      };

      connection.Close();
    }

    return newBoard;
  }

  public void UpdateBoard(int id, UpdateBoardViewModel board)
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
          string.IsNullOrEmpty(board.Name) ? DBNull.Value : board.Name);
      command.Parameters.AddWithValue("@Descripcion",
          string.IsNullOrEmpty(board.Description) ? DBNull.Value : board.Description);

      int updatedBoard = command.ExecuteNonQuery();

      if (updatedBoard == 0)
      {
        throw new KeyNotFoundException($"No se encontro el tablero con ID: {id}.");
      }

      connection.Close();
    }
  }

  public int GetOwnerUserId(int id)
  {
    string query = @"SELECT 
                      id_usuario_propietario 
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

  public GetBoardViewModel GetBoardId(int id)
  {
    GetBoardViewModel boardFound = null;

    string query = @"SELECT 
                      t.id, 
                      u.nombre_de_usuario, 
                      t.nombre, 
                      t.descripcion 
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
          boardFound = new GetBoardViewModel
          {
            Id = reader.GetInt32(0),
            OwnerUserName = reader.GetString(1),
            Name = reader.GetString(2),
            Description = reader.GetString(3)
          };
        }
      }

      connection.Close();
    }

    if (boardFound == null)
    {
      throw new KeyNotFoundException($"No se encontro el tablero con ID: {id}.");
    }

    return boardFound;
  }

  public List<GetBoardViewModel> GetBoards()
  {
    List<GetBoardViewModel> boards = new List<GetBoardViewModel>();

    string query = @"SELECT 
                      t.id, 
                      u.nombre_de_usuario, 
                      t.nombre, 
                      t.descripcion 
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
          boards.Add(new GetBoardViewModel
          {
            Id = reader.GetInt32(0),
            OwnerUserName = reader.GetString(1),
            Name = reader.GetString(2),
            Description = reader.GetString(3)
          });
        }
      }

      connection.Close();
    }

    return boards;
  }

  public List<GetBoardViewModel> GetBoardsByUserId(int idUser)
  {
    List<GetBoardViewModel> boardsFound = new List<GetBoardViewModel>();

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

      command.Parameters.AddWithValue("@Id", idUser);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          boardsFound.Add(new GetBoardViewModel
          {
            Id = reader.GetInt32(0),
            OwnerUserName = reader.GetString(1),
            Name = reader.GetString(2),
            Description = reader.GetString(3)
          });
        }
      }

      connection.Close();
    }

    return boardsFound;
  }

  public void DeleteBoard(int id)
  {
    if (HasTasks(id))
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

      int rowsAffected = command.ExecuteNonQuery();

      if (rowsAffected == 0)
      {
        throw new KeyNotFoundException($"No se encontro el tablero con ID: {id}.");
      }

      connection.Close();
    }
  }

  private bool HasTasks(int id)
  {
    int count;

    string query = @"SELECT 
                      COUNT(*) 
                     FROM Tarea 
                     WHERE id_tablero = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);
      count = Convert.ToInt32(command.ExecuteScalar());

      connection.Close();
    }

    return count > 0;
  }
}
