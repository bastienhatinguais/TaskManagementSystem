using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Api.Entities;

public class Task
{
    public Guid Id { get; set; }

    public required string Title { get; set; }
    
    public string Description { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; } = false;
}
