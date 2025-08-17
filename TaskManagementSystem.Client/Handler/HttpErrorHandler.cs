using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManagementSystem.Client.Exceptions;

namespace TaskManagementSystem.Client.Handler;

public class HttpErrorHandler : DelegatingHandler
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
            return response;

        ProblemDetails problem;
        try
        {
            problem = JsonSerializer.Deserialize<ProblemDetails>(await response.Content.ReadAsStringAsync(cancellationToken), _jsonOptions) ?? new ProblemDetails();
        }
        catch
        {
            problem = new ProblemDetails();
        }

        throw new ApiException(problem, response.StatusCode);
    }
}
