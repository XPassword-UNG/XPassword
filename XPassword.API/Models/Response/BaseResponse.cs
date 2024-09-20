namespace XPassword.API.Models.Response;

public class BaseResponse
{
    public bool Success { get; set; } = true;
    public string? Error { get; set; }
}