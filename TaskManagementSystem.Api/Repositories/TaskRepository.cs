using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Api.Repositories.Interfaces;

namespace TaskManagementSystem.Api.Repositories;

public class TaskRepository(AppDbContext appDbContext) : BaseRepository<ToDoTask>(appDbContext), ITaskRepository
{
}
