using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Api.Entities;

public class ToDoTask
{
    public Guid Id { get; set; }

    [StringLength(140)]
    public required string Title { get; set; }

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; } = false;
}
