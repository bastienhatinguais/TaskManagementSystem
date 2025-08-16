using AutoMapper;
using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Api.Repositories.Interfaces;
using TaskManagementSystem.Api.Services.Interfaces;
using TaskManagementSystem.Shared.DTOs.ToDoTask;

namespace TaskManagementSystem.Api.Services;
public class TaskService(ITaskRepository taskRepository, IMapper mapper, ITaskNotificationService taskNotificationService) : ITaskService
{
    public Task<ToDoTask?> GetTaskByIdAsync(Guid id) => taskRepository.GetByIdAsync(id);

    public async Task<List<ToDoTask>> GetAllTasksAsync() => (await taskRepository.GetAllAsync())
        .OrderBy(t => t.IsCompleted)
        .ThenBy(t => t.DueDate)          
        .ToList();

    public async Task<ToDoTask> AddTaskAsync(ToDoTaskUpsertDto task)
    {
        var added = await taskRepository.AddAsync(mapper.Map<ToDoTask>(task));
        await taskRepository.SaveAllAsync();
        await taskNotificationService.NotifyTaskCreated(mapper.Map<ToDoTaskDto>(added));
        return added;
    }

    public async Task<ToDoTask> UpdateTaskAsync(Guid id, ToDoTaskUpsertDto task)
    {
        var existingTask = await taskRepository.GetByIdAsync(id) ?? throw new InvalidOperationException($"Task with id {id} not found.");
        mapper.Map(task, existingTask);
        await taskRepository.SaveAllAsync();
        await taskNotificationService.NotifyTaskUpdated(mapper.Map<ToDoTaskDto>(existingTask));
        return existingTask;
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        var existingTask = await taskRepository.GetByIdAsync(id) ?? throw new InvalidOperationException($"Task with id {id} not found.");
        taskRepository.Delete(existingTask);
        var result = await taskRepository.SaveAllAsync();
        await taskNotificationService.NotifyTaskDeleted(id);
        return result;
    }

}
