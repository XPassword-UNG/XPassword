namespace XPassword.API.Models;

[Serializable]
public sealed class Register
{
    public long Id { get; set; }
    public long UserId { get; set; } = -1L;
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public string? Password { get; set; }
}