using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Api.Repositories.Interfaces;
using TaskManagementSystem.Api.Services.Interfaces;

namespace TaskManagementSystem.Api.Services;
public class TaskService(ITaskRepository taskRepository) : ITaskService
{
    public Task<ToDoTask?> GetTaskByIdAsync(int id) => taskRepository.GetByIdAsync(id);

    public Task<List<ToDoTask>> GetAllTasksAsync() => taskRepository.GetAllAsync();

    public Task<ToDoTask> AddTaskAsync(ToDoTask task) => taskRepository.AddAsync(task);

    public ToDoTask UpdateTask(ToDoTask task) => taskRepository.Update(task);
}
