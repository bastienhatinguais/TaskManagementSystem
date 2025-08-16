using TaskManagementSystem.Shared.DTOs.ToDoTask;

namespace TaskManagementSystem.Api.Services.Interfaces;
public interface ITaskNotificationService
{
    Task NotifyTaskCreated(ToDoTaskDto task);
    Task NotifyTaskDeleted(Guid id);
    Task NotifyTaskUpdated(ToDoTaskDto task);
}