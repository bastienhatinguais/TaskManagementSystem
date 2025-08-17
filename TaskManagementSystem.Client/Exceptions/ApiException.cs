using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TaskManagementSystem.Client.Exceptions
{
    public sealed class ApiException : Exception
    {
        public ProblemDetails Problem { get; }
        public HttpStatusCode StatusCode { get; }

        public ApiException(ProblemDetails problem, HttpStatusCode statusCode, Exception? inner = null)
            : base(problem.Title ?? problem.Detail ?? $"HTTP {(int)statusCode}", inner)
        {
            Problem = problem;
            StatusCode = statusCode;
        }
    }
}