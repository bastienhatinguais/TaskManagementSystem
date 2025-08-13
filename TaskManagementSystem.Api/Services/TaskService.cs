using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Api.Repositories.Interfaces;
using TaskManagementSystem.Api.Services.Interfaces;

namespace TaskManagementSystem.Api.Services;
public class TaskService(ITaskRepository taskRepository) : ITaskService
{
    public Task<ToDoTask?> GetTaskByIdAsync(int id) => taskRepository.GetByIdAsync(id);

    public Task<List<ToDoTask>> GetAllTasksAsync() => taskRepository.GetAllAsync();

    public async Task<ToDoTask> AddTaskAsync(ToDoTask task)
    {
        var added = await taskRepository.AddAsync(task);
        await taskRepository.SaveAllAsync(); 
        return added;
    }

    public async Task<ToDoTask> UpdateTaskAsync(ToDoTask task)
    {
        var updated = taskRepository.Update(task);
        await taskRepository.SaveAllAsync(); 
        return updated;
    }
}
