using TaskManagementSystem.Shared.DTOs.ToDoTask;

namespace TaskManagementSystem.Client.Services;
public interface ITaskService
{
    Task<HttpResponseMessage> CreateAsync(ToDoTaskUpsertDto dto);
    Task<HttpResponseMessage> DeleteAsync(Guid id);
    Task<ToDoTaskDto?> GetAsync(Guid id);
    Task<List<ToDoTaskDto>?> ListAsync();
    Task<HttpResponseMessage> UpdateAsync(Guid id, ToDoTaskUpsertDto dto);
}