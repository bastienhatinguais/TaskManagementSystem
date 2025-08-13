using TaskManagementSystem.Api.Entities;

namespace TaskManagementSystem.Api.Services.Interfaces;

public interface ITaskService
{
    Task<ToDoTask> AddTaskAsync(ToDoTask task);
    Task<List<ToDoTask>> GetAllTasksAsync();
    Task<ToDoTask?> GetTaskByIdAsync(int id);
    Task<ToDoTask> UpdateTaskAsync(ToDoTask task);
}