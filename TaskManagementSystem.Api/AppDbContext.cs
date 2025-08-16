namespace TaskManagementSystem.Api;

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Api.Seeds;

public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
    public DbSet<ToDoTask> Tasks => Set<ToDoTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ToDoTaskSeed.Initialize(modelBuilder);
    }
}