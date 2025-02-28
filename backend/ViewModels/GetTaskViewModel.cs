using Kanban.Enums;

namespace Kanban.ViewModels;

public class GetTaskViewModel
{
  public int Id { get; set; }
  public string Name { get; set; }
  public StatusTask Status { get; set; }
  public string Description { get; set; }
  public string Color { get; set; }
  public int AssignedUserId { get; set; }
  public string AssignedUserName { get; set; } = "";
}
