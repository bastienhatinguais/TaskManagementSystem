using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagementSystem.Api;
using TaskManagementSystem.Api.Hubs;
using TaskManagementSystem.Api.Repositories;
using TaskManagementSystem.Api.Repositories.Interfaces;
using TaskManagementSystem.Api.Services;
using TaskManagementSystem.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskNotificationService, TaskNotificationService>();

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(x =>
    {
        x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:7201", "https://localhost:7201");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<TaskHub>(TaskHub.HubUrl);

app.Run();
