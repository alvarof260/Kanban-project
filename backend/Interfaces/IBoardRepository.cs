using Kanban.Models;
using Kanban.ViewModels;

namespace Kanban.Interfaces;

public interface IBoardRepository
{
  public Board CreateBoard(CreateBoardViewModel board);
  public void UpdateBoard(int id, UpdateBoardViewModel board);
  public int GetOwnerUserId(int id);
  public GetBoardViewModel GetBoardId(int id);
  public List<GetBoardViewModel> GetBoards();
  public List<GetBoardViewModel> GetBoardsByUserId(int idUser);
  public void DeleteBoard(int id);
}
