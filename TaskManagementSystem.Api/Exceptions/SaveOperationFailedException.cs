namespace TaskManagementSystem.Api.Exceptions;

public class SaveOperationFailedException : Exception
{
    public SaveOperationFailedException(string operation)
        : base($"Failed to save the operation : {operation}.") { }

    public SaveOperationFailedException(string operation, Exception innerException)
        : base($"Failed to save the operation :  {operation}.", innerException) { }
}