using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Shared.DTOs;
using TaskManagementSystem.Shared.DTOs.ToDoTask;

namespace TaskManagementSystem.Api.Services.Interfaces;

public interface ITaskService
{
    Task<ToDoTask> AddTaskAsync(ToDoTaskUpsertDto task);
    Task<bool> DeleteTaskAsync(Guid id);
    Task<List<ToDoTask>> GetAllTasksAsync();
    Task<ToDoTask?> GetTaskByIdAsync(Guid id);
    Task<ToDoTask> UpdateTaskAsync(Guid id, ToDoTaskUpsertDto task);
}