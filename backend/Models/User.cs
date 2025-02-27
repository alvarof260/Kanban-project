using Kanban.Enums;

namespace Kanban.Models;

public class User
{
  private int _id;
  private string _username;
  private string _password;
  private RoleUser _roleUser;

  public int Id { get => _id; set => _id = value; }
  public string Username { get => _username; set => _username = value; }
  public string Password { get => _password; set => _password = value; }
  public RoleUser RoleUser { get => _roleUser; set => _roleUser = value; }

  public User(int id, string username, string password, RoleUser roleUser)
  {
    this.Id = id;
    this.Username = username;
    this.Password = password;
    this.RoleUser = roleUser;
  }

  public User() { }
}
