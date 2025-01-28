using Kanban.Models;
using Kanban.ViewModels;
using Kanban.Enums;
using Kanban.Interfaces;
using Microsoft.Data.Sqlite;

namespace Kanban.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
  private readonly string _connectionString;

  public UsuarioRepository(string connectionString)
  {
    this._connectionString = connectionString;
  }

  public Usuario CreateUsuario(CreateUsuarioViewModel usuario)
  {
    Usuario nuevoUsuario = null;

    string query = @"INSERT INTO Usuario 
                     (nombre_de_usuario, password, rol_usuario) 
                     VALUES (@NombreDeUsuario, @Password, @RolUsuario);
                     SELECT last_insert_rowid();"; // obtener el id del ultimo usuario creado

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@NombreDeUsuario", usuario.Usuario);
      command.Parameters.AddWithValue("@Password", usuario.Password);
      command.Parameters.AddWithValue("@RolUsuario", usuario.RolUsuario);

      int idGenerado = Convert.ToInt32(command.ExecuteScalar());

      nuevoUsuario = new Usuario
      {
        Id = idGenerado,
        NombreDeUsuario = usuario.Usuario,
        Password = usuario.Password,
        RolUsuario = usuario.RolUsuario
      };

      connection.Close();
    }

    return nuevoUsuario;
  }

  public void UpdateUsuario(int id, UpdateUsuarioViewModel usuario)
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
          string.IsNullOrEmpty(usuario.Usuario) ? DBNull.Value : usuario.Usuario);
      command.Parameters.AddWithValue("@Password",
          string.IsNullOrEmpty(usuario.Password) ? DBNull.Value : usuario.Password);
      command.Parameters.AddWithValue("@RolUsuario",
          usuario.RolUsuario.HasValue ? usuario.RolUsuario : DBNull.Value);

      int usuarioUpdated = command.ExecuteNonQuery();

      if (usuarioUpdated == 0)
      {
        throw new Exception($"No se encontro el usuario con ID: {id}.");
      }

      connection.Close();
    }
  }

  public List<GetUsuariosViewModel> GetUsuarios()
  {
    List<GetUsuariosViewModel> usuarios = new List<GetUsuariosViewModel>();

    string query = @"SELECT nombre_de_usuario, rol_usuario 
                     FROM Usuario;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          usuarios.Add(new GetUsuariosViewModel
          {
            Usuario = reader.GetString(0),
            RolUsuario = (RolUsuario)reader.GetInt32(1)
          });
        }
      }
      connection.Close();
    }

    return usuarios;
  }

  public Usuario GetUsuarioId(int id)
  {
    Usuario usuarioBuscado = null;

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
          usuarioBuscado = new Usuario
          {
            Id = reader.GetInt32(0),
            NombreDeUsuario = reader.GetString(1),
            Password = reader.GetString(2),
            RolUsuario = (RolUsuario)reader.GetInt32(3)
          };
        }
      }

      connection.Close();
    }

    if (usuarioBuscado == null)
    {
      throw new Exception($"No se encontro el usuario con ID: {id}.");
    }

    return usuarioBuscado;
  }

  // TODO: debo cambiar nombre, ver logica y lanzar errores
  public Usuario ObtenerUsuarioNombre(string nombre)
  {
    Usuario usuarioBuscado = new Usuario();
    string query = @"SELECT * FROM Usuario WHERE nombre_de_usuario = @NombreDeUsuario;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@NombreDeUsuario", nombre);
      using (SqliteDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          usuarioBuscado.Id = reader.GetInt32(0);
          usuarioBuscado.NombreDeUsuario = reader.GetString(1);
          usuarioBuscado.Password = reader.GetString(2);
          usuarioBuscado.RolUsuario = (RolUsuario)reader.GetInt32(3);
        }
      }

      connection.Close();
    }

    return usuarioBuscado;
  }


  public void DeleteUsuario(int id)
  {
    if (!Verificar(id))
    {
      throw new InvalidOperationException("El usuario está asociado a tableros o tareas y no puede ser eliminado.");
    }

    string query = @"DELETE FROM Usuario WHERE id = @Id;";
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);

      int filasAfectadas = command.ExecuteNonQuery();

      if (filasAfectadas == 0)
      {
        throw new Exception($"No se encontro el usuario con ID: {id}.");
      }

      connection.Close();
    }
  }

  public void ChangePassword(int id, Usuario usuario)
  {
    string query = @"UPDATE Usuario 
                     SET password = @Password 
                     WHERE id = @Id;";

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      command.Parameters.AddWithValue("@Id", id);
      command.Parameters.AddWithValue("@Password", usuario.Password);

      int usuarioUpdated = command.ExecuteNonQuery();

      if (usuarioUpdated == 0)
      {
        throw new Exception("No se encontro el usuario con id: {id}");
      }

      connection.Close();
    }
  }

  private bool Verificar(int id)
  {
    int contador;

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

      contador = Convert.ToInt32(command.ExecuteScalar());

      connection.Close();
    }

    return contador == 0; // El usuario no tiene tareas o tableros
  }
}
