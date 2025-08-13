using AutoMapper;
using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Shared.DTOs;

namespace TaskManagementSystem.Api.MapperProfiles;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskDto, ToDoTask>().ReverseMap();
    }
}
