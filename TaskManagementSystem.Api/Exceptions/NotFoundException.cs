namespace TaskManagementSystem.Api.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string resourceType, object id)
        : base($"{resourceType} with id {id} was not found.") { }
}
