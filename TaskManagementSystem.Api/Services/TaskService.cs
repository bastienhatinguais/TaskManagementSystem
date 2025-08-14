using AutoMapper;
using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Api.Repositories.Interfaces;
using TaskManagementSystem.Api.Services.Interfaces;
using TaskManagementSystem.Shared.DTOs.ToDoTask;

namespace TaskManagementSystem.Api.Services;
public class TaskService(ITaskRepository taskRepository, IMapper mapper) : ITaskService
{
    public Task<ToDoTask?> GetTaskByIdAsync(Guid id) => taskRepository.GetByIdAsync(id);

    public Task<List<ToDoTask>> GetAllTasksAsync() => taskRepository.GetAllAsync();

    public async Task<ToDoTask> AddTaskAsync(ToDoTaskUpsertDto task)
    {
        var added = await taskRepository.AddAsync(mapper.Map<ToDoTask>(task));
        await taskRepository.SaveAllAsync(); 
        return added;
    }

    public async Task<ToDoTask> UpdateTaskAsync(Guid id, ToDoTaskUpsertDto task)
    {
        var existingTask = await taskRepository.GetByIdAsync(id);
        if (existingTask == null)
        {
            throw new InvalidOperationException($"Task with id {id} not found.");
        }
        mapper.Map(task, existingTask);

        await taskRepository.SaveAllAsync(); 
        return existingTask;
    }
}
