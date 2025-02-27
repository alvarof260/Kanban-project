using Kanban.Models;
using Kanban.ViewModels;
using Kanban.Enums;
using Kanban.Interfaces;
using Microsoft.Data.Sqlite;

namespace Kanban.Repositories;

public class UserRepository : IUserRepository
{
  private readonly string _connectionString;

  public UserRepository(string connectionString)
  {
    this._connectionString = connectionString;
  }

  public User CreateUser(CreateUserViewModel user)
  {
    User newUser = null;

    string query = @"INSERT INTO Usuario 
                     (nombre_de_usuario, password, rol_usuario) 
                     VALUES (@NombreDeUsuario, @Password, @RolUsuario);
                     SELECT last_insert_rowid();";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@NombreDeUsuario", user.Username);
      command.Parameters.AddWithValue("@Password", user.Password);
      command.Parameters.AddWithValue("@RolUsuario", user.RoleUser);

      int id = Convert.ToInt32(command.ExecuteScalar());

      newUser = new User
      {
        Id = id,
        Username = user.Username,
        Password = user.Password,
        RoleUser = (RoleUser)user.RoleUser
      };

      connection.Close();
    }

    return newUser;
  }

  public void UpdateUser(int id, UpdateUserViewModel user)
  {
    string query = @"UPDATE Usuario 
                     SET 
                     nombre_de_usuario = COALESCE(@NombreDeUsuario, nombre_de_usuario),
                     password = COALESCE(@Password, password),
                     rol_usuario = COALESCE(@RolUsuario, rol_usuario)
                     WHERE id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@Id", id);

      command.Parameters.AddWithValue("@NombreDeUsuario",
          string.IsNullOrEmpty(user.Username) ? DBNull.Value : user.Username);
      command.Parameters.AddWithValue("@Password",
          string.IsNullOrEmpty(user.Password) ? DBNull.Value : user.Password);
      command.Parameters.AddWithValue("@RolUsuario",
          user.RoleUser.HasValue ? user.RoleUser : DBNull.Value);

      int UpdatedUser = command.ExecuteNonQuery();

      if (UpdatedUser == 0)
      {
        throw new KeyNotFoundException($"No se encontro el usuario con ID: {id}.");
      }

      connection.Close();
    }
  }

  public List<GetUserViewModel> GetUsers()
  {
    List<GetUserViewModel> users = new List<GetUserViewModel>();

    string query = @"SELECT 
                     id,
                     nombre_de_usuario,
                     rol_usuario
                     FROM Usuario;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          users.Add(new GetUserViewModel
          {
            Id = reader.GetInt32(0),
            Username = reader.GetString(1),
            RoleUser = (RoleUser)reader.GetInt32(2)
          });
        }
      }
      connection.Close();
    }

    return users;
  }

  public User GetUserId(int id)
  {
    User userFound = null;

    string query = @"SELECT * 
                     FROM Usuario 
                     WHERE id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);
      using (SqliteDataReader reader = command.ExecuteReader())
      {
        if (reader.Read())
        {
          userFound = new User
          {
            Id = reader.GetInt32(0),
            Username = reader.GetString(1),
            Password = reader.GetString(2),
            RoleUser = (RoleUser)reader.GetInt32(3)
          };
        }
      }

      connection.Close();
    }

    if (userFound == null)
    {
      throw new KeyNotFoundException($"No se encontro el usuario con ID: {id}.");
    }

    return userFound;
  }

  public User GetUserLogin(string username)
  {
    User userFound = null;

    string query = @"SELECT * FROM 
                     Usuario 
                     WHERE nombre_de_usuario = @NombreDeUsuario;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@NombreDeUsuario", username);
      using (SqliteDataReader reader = command.ExecuteReader())
      {
        if (reader.Read())
        {
          userFound = new User
          {
            Id = reader.GetInt32(0),
            Username = reader.GetString(1),
            Password = reader.GetString(2),
            RoleUser = (RoleUser)reader.GetInt32(3)
          };
        }
      }

      connection.Close();
    }

    if (userFound == null)
    {
      throw new KeyNotFoundException($"No se encontro el usuario con nombre: {username}.");
    }

    return userFound;
  }

  public void DeleteUser(int id)
  {
    if (!UserHasTasksOrBoards(id))
    {
      throw new InvalidOperationException("El usuario está asociado a tableros o tareas y no puede ser eliminado.");
    }

    string query = @"DELETE FROM Usuario 
                     WHERE id = @Id;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);

      int rowsAffected = command.ExecuteNonQuery();

      if (rowsAffected == 0)
      {
        throw new KeyNotFoundException($"No se encontro el usuario con ID: {id}.");
      }

      connection.Close();
    }
  }

  public void ChangePassword(int id, User user)
  {
    string query = @"UPDATE Usuario 
                     SET password = @Password 
                     WHERE id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);
      command.Parameters.AddWithValue("@Password", user.Password);

      int rowsAffected = command.ExecuteNonQuery();

      if (rowsAffected == 0)
      {
        throw new KeyNotFoundException($"No se encontro el usuario con id: {id}");
      }

      connection.Close();
    }
  }

  private bool UserHasTasksOrBoards(int id)
  {
    int count;

    string query = @"SELECT COUNT(*)
                     FROM Usuario u
                     LEFT JOIN Tablero t ON u.id = id_usuario_propietario
                     LEFT JOIN Tarea ta ON u.id = ta.id_usuario_asignado
                     WHERE u.id = @Id AND (t.id IS NOT NULL OR ta.id IS NOT NULL);";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);

      count = Convert.ToInt32(command.ExecuteScalar());

      connection.Close();
    }

    return count == 0; // El usuario no tiene tareas o tableros
  }
}
