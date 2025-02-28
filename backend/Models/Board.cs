
namespace Kanban.Models;

public class Board
{
  private int _id;
  private int _ownerUserId;
  private string _name;
  private string _description;

  public int Id { get => _id; set => _id = value; }
  public int OwnerUserId { get => _ownerUserId; set => _ownerUserId = value; }
  public string Name { get => _name; set => _name = value; }
  public string Description { get => _description; set => _description = value; }

  public Board(int id, int ownerUserId, string name, string description)
  {
    this.Id = id;
    this.OwnerUserId = ownerUserId;
    this.Name = name;
    this.Description = description;
  }

  public Board() { }
}
