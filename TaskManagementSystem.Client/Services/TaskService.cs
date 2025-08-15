using System.Net.Http.Json;
using TaskManagementSystem.Shared.DTOs.ToDoTask;

namespace TaskManagementSystem.Client.Services;

public class TaskService(HttpClient http) : ITaskService
{
    private const string _baseUrl = "api/task";

    public Task<List<ToDoTaskDto>?> ListAsync()
        => http.GetFromJsonAsync<List<ToDoTaskDto>>(_baseUrl);

    public Task<ToDoTaskDto?> GetAsync(Guid id)
        => http.GetFromJsonAsync<ToDoTaskDto?>($"{_baseUrl}/{id}");

    public Task<HttpResponseMessage> CreateAsync(ToDoTaskUpsertDto dto)
        => http.PostAsJsonAsync(_baseUrl, dto);

    public Task<HttpResponseMessage> UpdateAsync(Guid id, ToDoTaskUpsertDto dto)
        => http.PutAsJsonAsync($"{_baseUrl}/{id}", dto);

    public Task<HttpResponseMessage> DeleteAsync(Guid id)
        => http.DeleteAsync($"{_baseUrl}/{id}");
}
