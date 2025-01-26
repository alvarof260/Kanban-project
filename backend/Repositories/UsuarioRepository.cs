using Microsoft.Data.Sqlite;
using Kanban.Models;
using Kanban.Enums;
using Kanban.DTO;

namespace Kanban.Repositories
{
  public interface IUsuarioRepository
  {
    public Usuario CrearUsuario(Usuario usuario);
    public void ModificarUsuario(int Id, UsuarioDTO usuario);
    public List<Usuario> ObtenerUsuarios();
    public Usuario ObtenerUsuarioId(int id);
    public void EliminarUsuario(int id);
    public void CambiarPassword(int id, Usuario usuario);
  }

  public class UsuarioRepository : IUsuarioRepository
  {
    private readonly string _connectionString;

    public UsuarioRepository(string connectionString)
    {
      this._connectionString = connectionString;
    }

    public Usuario CrearUsuario(Usuario usuario)
    {
      string query = @"INSERT INTO Usuario (nombre_de_usuario, password, rol_usuario) VALUES (@NombreDeUsuario, @Password, @RolUsuario);
                       SELECT last_insert_rowid();";
      using (SqliteConnection connection = new SqliteConnection(_connectionString))
      {
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@NombreDeUsuario", usuario.NombreDeUsuario);
        command.Parameters.AddWithValue("@Password", usuario.Password);
        command.Parameters.AddWithValue("@RolUsuario", usuario.RolUsuario);

        int idGenerado = Convert.ToInt32(command.ExecuteScalar());
        usuario.Id = idGenerado;

        connection.Close();
      }
      return usuario;
    }

    public void ModificarUsuario(int id, UsuarioDTO usuario)
    {
      string query = @"
        UPDATE Usuario 
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

        // Asigna valores solo si están presentes en el DTO
        command.Parameters.AddWithValue("@NombreDeUsuario",
            string.IsNullOrEmpty(usuario.NombreDeUsuario) ? DBNull.Value : usuario.NombreDeUsuario);
        command.Parameters.AddWithValue("@Password",
            string.IsNullOrEmpty(usuario.Password) ? DBNull.Value : usuario.Password);
        command.Parameters.AddWithValue("@RolUsuario",
            usuario.RolUsuario.HasValue ? usuario.RolUsuario : DBNull.Value);

        command.ExecuteNonQuery();
        connection.Close();
      }
    }

    public List<Usuario> ObtenerUsuarios()
    {
      List<Usuario> usuarios = new List<Usuario>();
      string query = @"SELECT id, nombre_de_usuario, rol_usuario FROM Usuario;";
      using (SqliteConnection connection = new SqliteConnection(_connectionString))
      {
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);

        using (SqliteDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            usuarios.Add(new Usuario
            {
              Id = reader.GetInt32(0),
              NombreDeUsuario = reader.GetString(1),
              RolUsuario = (RolUsuario)reader.GetInt32(2)
            });
          }
        }
        connection.Close();
      }

      return usuarios;
    }

    public Usuario ObtenerUsuarioId(int id)
    {
      Usuario usuarioBuscado = new Usuario();
      string query = @"SELECT nombre_de_usuario, password, rol_usuario FROM Usuario WHERE id = @Id;";
      using (SqliteConnection connection = new SqliteConnection(_connectionString))
      {
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);
        using (SqliteDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            usuarioBuscado.Id = reader.GetInt32(0);
            usuarioBuscado.NombreDeUsuario = reader.GetString(1);
            usuarioBuscado.RolUsuario = (RolUsuario)reader.GetInt32(2);
          }
        }

        connection.Close();
      }
      return usuarioBuscado;
    }

    public void EliminarUsuario(int id)
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
        command.ExecuteNonQuery();

        connection.Close();
      }
    }

    public void CambiarPassword(int id, Usuario usuario)
    {
      string query = @"UPDATE Usuario SET password = @Password WHERE id = @Id;";
      using (SqliteConnection connection = new SqliteConnection(_connectionString))
      {
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Password", usuario.Password);

        command.ExecuteNonQuery();

        connection.Close();
      }
    }

    private bool Verificar(int id)
    {
      int contador;
      string query = @"
        SELECT COUNT(*)
        FROM Usuario u
        LEFT JOIN Tablero t ON u.id = id_usuario_propietario
        LEFT JOIN Tarea ta ON u.id = ta.id_usuario_asignado
        WHERE u.id = @Id;";
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
}
