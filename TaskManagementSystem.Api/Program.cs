using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Api;
using TaskManagementSystem.Api.Repositories;
using TaskManagementSystem.Api.Repositories.Interfaces;
using TaskManagementSystem.Api.Services;
using TaskManagementSystem.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
