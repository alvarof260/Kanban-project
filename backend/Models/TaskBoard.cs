using Kanban.Enums;

namespace Kanban.Models;

public class TaskBoard
{
  private int _id;
  private int _boardId;
  private string _name;
  private string _description;
  private string _color;
  private StatusTask _status;
  private int? _assignedUserId;

  public int Id { get => _id; set => _id = value; }
  public int BoardId { get => _boardId; set => _boardId = value; }
  public string Name { get => _name; set => _name = value; }
  public string Description { get => _description; set => _description = value; }
  public string Color { get => _color; set => _color = value; }
  public StatusTask Status { get => _status; set => _status = value; }
  public int? AssignedUserId { get => _assignedUserId; set => _assignedUserId = value; }

  public TaskBoard(int id, int boardId, string name, string description, string color, StatusTask status, int assignedUserId)
  {
    this.Id = id;
    this.BoardId = boardId;
    this.Name = name;
    this.Description = description;
    this.Color = color;
    this.Status = status;
    this.AssignedUserId = assignedUserId;
  }

  public TaskBoard() { }
}
