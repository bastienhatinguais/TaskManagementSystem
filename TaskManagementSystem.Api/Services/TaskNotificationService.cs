using Microsoft.AspNetCore.SignalR;
using TaskManagementSystem.Api.Hubs;
using TaskManagementSystem.Api.Services.Interfaces;
using TaskManagementSystem.Shared.DTOs.ToDoTask;

namespace TaskManagementSystem.Api.Services;

public class TaskNotificationService(IHubContext<TaskHub> hubContext) : ITaskNotificationService
{
    public async Task NotifyTaskCreated(ToDoTaskDto task)
    {
        await hubContext.Clients.All.SendAsync("TaskCreated", task);
    }

    public async Task NotifyTaskUpdated(ToDoTaskDto task)
    {
        await hubContext.Clients.All.SendAsync("TaskUpdated", task);
    }

    public async Task NotifyTaskDeleted(Guid id)
    {
        await hubContext.Clients.All.SendAsync("TaskDeleted", id);
    }
}