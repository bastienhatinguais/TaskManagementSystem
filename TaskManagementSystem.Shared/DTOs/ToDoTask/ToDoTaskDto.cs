using System;
namespace TaskManagementSystem.Shared.DTOs.ToDoTask;

public class ToDoTaskDto : ToDoTaskUpsertDto
{
    public Guid Id { get; set; }
}
