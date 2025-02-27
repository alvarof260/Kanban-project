using Kanban.Models;
using Kanban.ViewModels;

namespace Kanban.Interfaces;

public interface IUserRepository
{
  public User CreateUser(CreateUserViewModel user);
  public void UpdateUser(int id, UpdateUserViewModel user);
  public List<GetUserViewModel> GetUsers();
  public User GetUserId(int id);
  public User GetUserLogin(string username);
  public void DeleteUser(int id);
  public void ChangePassword(int id, User user);
}
