namespace XPassword.API.Models.Response;

public class TokenResponse : BaseResponse
{
    public string Token { get; set; } = "";
    public int LifeTime { get; set; }
}