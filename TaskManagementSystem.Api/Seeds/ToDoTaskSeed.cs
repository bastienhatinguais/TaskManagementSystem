using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Api.Entities;

namespace TaskManagementSystem.Api.Seeds;

public static class ToDoTaskSeed
{
    public static void Initialize(ModelBuilder modelBuilder)
    {
        var tasks = new List<ToDoTask>
{
    new()
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Title = "Finish project report",
        Description = "Complete the final draft of the project report and send it to the manager.",
        DueDate = new DateTime(2025, 08, 19, 10, 0, 0),
        IsCompleted = false
    },
    new()
    {
        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Title = "Grocery shopping",
        Description = "Buy vegetables, fruits, and milk for the week.",
        DueDate = new DateTime(2025, 08, 17, 18, 0, 0),
        IsCompleted = false
    },
    new()
    {
        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        Title = "Dentist appointment",
        Description = "Routine dental check-up at the downtown clinic.",
        DueDate = new DateTime(2025, 08, 23, 08, 30, 0),
        IsCompleted = false
    },
    new()
    {
        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
        Title = "Pay electricity bill",
        Description = "Pay the monthly electricity bill before the due date.",
        DueDate = new DateTime(2025, 08, 14, 23, 59, 0),
        IsCompleted = true
    },
    new()
    {
        Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
        Title = "Team meeting",
        Description = "Weekly sync-up meeting with the development team.",
        DueDate = new DateTime(2025, 08, 16, 20, 0, 0),
        IsCompleted = false
    }
};


        modelBuilder.Entity<ToDoTask>().HasData(tasks);
    }
}