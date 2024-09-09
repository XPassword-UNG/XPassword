namespace XPassword.API.Models
{
    public sealed class ValidationException : Exception
    {
        private readonly List<ExceptMessage> _errors = new List<ExceptMessage>();

        public ValidationException() { }

        public ValidationException(Exception ex, string message)
        {
            _errors =
            [
                new ExceptMessage() { Exception = ex, Message = message }
            ];
        }

        public List<ExceptMessage> ErrosList => _errors;

        public void AddError(string message) => AddError(new Exception(message), message);

        public void AddError(Exception exception) => AddError(exception, exception.Message);

        public void AddError(Exception exception, string message) => _errors.Add(new ExceptMessage() { Exception = exception, Message = message });
    }
}