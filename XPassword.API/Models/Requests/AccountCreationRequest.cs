namespace XPassword.API.Models.Requests;

[Serializable]
public class AccountCreationRequest
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}