using Microsoft.Data.Sqlite;
using Kanban.Models;
using Kanban.Enums;

namespace Kanban.Repositories
{
  public interface IUsuarioRepository
  {
    public void CrearUsuario(Usuario usuario);
    public void ModificarUsuario(int Id, Usuario usuario);
    public List<Usuario> ObtenerUsuarios();
    public Usuario ObtenerUsuarioId(int id);
    public void EliminarUsuario(int id);
  }

  public class UsuarioRepository : IUsuarioRepository
  {
    private readonly string _connectionString;

    public UsuarioRepository(string connectionString)
    {
      this._connectionString = connectionString;
    }

    public void CrearUsuario(Usuario usuario)
    {
      string query = @"INSERT INTO Usuario (nombre_de_usuario, password, rol_usuario) VALUES (@NombreDeUsuario, @Password, @RolUsuario);";
      using (SqliteConnection connection = new SqliteConnection(_connectionString))
      {
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@NombreDeUsuario", usuario.NombreDeUsuario);
        command.Parameters.AddWithValue("@Password", usuario.Password);
        command.Parameters.AddWithValue("@RolUsuario", usuario.RolUsuario);

        command.ExecuteNonQuery();

        connection.Close();
      }
    }

    public void ModificarUsuario(int id, Usuario usuario)
    {
      string query = @"UPDATE Usuario SET nombre_de_usuario = @NombreDeUsuario, rol_usuario = @RolUsuario WHERE id = @Id;";
      using (SqliteConnection connection = new SqliteConnection(_connectionString))
      {
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@NombreDeUsuario", usuario.NombreDeUsuario);
        command.Parameters.AddWithValue("@RolUsuario", usuario.RolUsuario);
        command.Parameters.AddWithValue("@id", usuario.Id);

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
      Usuario usuarioBuscado = null;
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
      if (Verificar(id))
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
