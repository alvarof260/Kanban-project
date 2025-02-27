using Kanban.Enums;

namespace Kanban.ViewModels;

public class GetUserViewModel
{
  public int Id { get; set; }
  public string Username { get; set; }
  public RoleUser RoleUser { get; set; }
}
