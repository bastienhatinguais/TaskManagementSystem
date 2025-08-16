using Microsoft.AspNetCore.SignalR;
namespace TaskManagementSystem.Api.Hubs;

public class TaskHub : Hub
{
    public const string HubUrl = "/hubs/task";
}