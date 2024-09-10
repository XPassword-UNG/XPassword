namespace XPassword.Database.Model.Exceptions;

[Serializable]
public record class ExceptMessage
{
    public required Exception Exception { get; init; }
    public required string Message { get; init; }
}