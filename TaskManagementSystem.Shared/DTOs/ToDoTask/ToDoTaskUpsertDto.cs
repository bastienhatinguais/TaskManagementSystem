using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Shared.DTOs.ToDoTask;

public class ToDoTaskUpsertDto
{
    [Required, StringLength(140)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; } = string.Empty;

    [Required, DataType(DataType.Date)]
    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }
}
