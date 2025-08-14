using AutoMapper;
using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Shared.DTOs.ToDoTask;

namespace TaskManagementSystem.Api.MapperProfiles;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<ToDoTaskDto, ToDoTask>().ReverseMap();
        CreateMap<ToDoTaskUpsertDto, ToDoTask>().ReverseMap();
    }
}
