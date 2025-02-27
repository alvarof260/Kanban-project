namespace Kanban.ViewModels.Response;

public class ApiResponse<T>
{
  public bool Success { get; set; }
  public string Message { get; set; }
  public T Data { get; set; }

  public ApiResponse(bool success, string message, T data)
  {
    this.Success = success;
    this.Message = message;
    this.Data = data;
  }
}
