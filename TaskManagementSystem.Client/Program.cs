using TaskManagementSystem.Client;
using TaskManagementSystem.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskManagementSystem.Client.Handler;
using TailBlazor.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBase = builder.Configuration["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;

builder.Services.AddTransient<HttpErrorHandler>();

builder.Services
    .AddHttpClient("API", client => client.BaseAddress = new Uri(apiBase))
    .AddHttpMessageHandler<HttpErrorHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));


builder.Services.AddScoped<ITaskService, TaskService>();

await builder.Build().RunAsync();
