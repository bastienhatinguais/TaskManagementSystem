namespace TaskManagementSystem.Api;

using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Api.Entities;

public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
    public DbSet<Task> Tasks => Set<Task>();
}