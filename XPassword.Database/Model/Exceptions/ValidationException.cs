namespace XPassword.Database.Model.Exceptions;

public sealed class ValidationException : Exception
{
    private readonly List<ExceptMessage> _errors = [];

    public ValidationException() { }

    public ValidationException(Exception ex, string message)
    {
        _errors =
        [
            new ExceptMessage() { Exception = ex, Message = message }
        ];
    }

    public List<ExceptMessage> ErrosList => _errors;

    public bool HasError => _errors.Count > 0;

    public void AddError(string message) => AddError(new Exception(message), message);

    public void AddError(Exception exception) => AddError(exception, exception.Message);

    public void AddError(Exception exception, string message) => _errors.Add(new ExceptMessage() { Exception = exception, Message = message });
}