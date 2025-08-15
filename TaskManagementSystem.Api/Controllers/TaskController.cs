using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Api.Services.Interfaces;
using TaskManagementSystem.Shared.DTOs.ToDoTask;

namespace TaskManagementSystem.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TaskController(ITaskService taskService, IMapper mapper) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var task = await taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<ToDoTaskDto>(task));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var tasks = await taskService.GetAllTasksAsync();
        return Ok(mapper.Map<List<ToDoTaskDto>>(tasks));
    }

    [HttpPost]
    public async Task<IActionResult> AddTask(ToDoTaskUpsertDto task)
    {
        var createdTask = await taskService.AddTaskAsync(task);
        return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, mapper.Map<ToDoTaskDto>(createdTask));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTask(Guid id, ToDoTaskUpsertDto task)
    {
        try
        {
            var  updatedTask = await taskService.UpdateTaskAsync(id, task);
            return Ok(mapper.Map<ToDoTaskDto>(updatedTask));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        try
        {
            await taskService.DeleteTaskAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
