namespace XPassword.Database.Model;

[Serializable]
public sealed class Account
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string HPassword { get; init; }
}