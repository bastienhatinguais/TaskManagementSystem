using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Api.Services.Interfaces;
using TaskManagementSystem.Shared.DTOs;

namespace TaskManagementSystem.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class TaskController(ITaskService taskService, IMapper mapper) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<TaskDto>(task));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var tasks = await taskService.GetAllTasksAsync();
        return Ok(mapper.Map<List<TaskDto>>(tasks));
    }

    [HttpPost]
    public async Task<IActionResult> AddTask(ToDoTask task)
    {
        if (task == null)
        {
            return BadRequest();
        }
        var createdTask = await taskService.AddTaskAsync(task);
        return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTaskAsync(int id, ToDoTask task)
    {
        if (task == null)
        {
            return BadRequest();
        }
        var existingTask = taskService.GetTaskByIdAsync(id).Result;
        if (existingTask == null)
        {
            return NotFound();
        }
        var updatedTask = await taskService.UpdateTaskAsync(task);
        return Ok(mapper.Map<TaskDto>(updatedTask));
    }
}
