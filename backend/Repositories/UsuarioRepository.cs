using Microsoft.Data.Sqlite;
using Kanban.Models;

namespace Kanban.Repositories
{
  public interface IUsuarioRepository
  {
    public void CrearUsuario(Usuario usuario);
    public void ModificarUsuario(int Id, Usuario usuario);
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

  }
}
