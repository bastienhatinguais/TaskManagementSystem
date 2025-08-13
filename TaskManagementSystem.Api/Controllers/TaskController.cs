using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Api.Services.Interfaces;

namespace TaskManagementSystem.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class TaskController(ITaskService taskService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var tasks = await taskService.GetAllTasksAsync();
        return Ok(tasks);
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
    public IActionResult UpdateTask(int id, ToDoTask task)
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
        var updatedTask = taskService.UpdateTask(task);
        return Ok(updatedTask);
    }


}
