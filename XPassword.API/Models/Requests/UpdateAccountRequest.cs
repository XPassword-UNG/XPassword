namespace XPassword.API.Models.Requests;

[Serializable]
public class AccountUpdateRequest
{
    public required string Username { get; init; }
    public required string Email { get; init; }
}