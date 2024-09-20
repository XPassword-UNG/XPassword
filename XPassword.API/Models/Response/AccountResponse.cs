using XPassword.Database.Model;

namespace XPassword.API.Models.Response;

public class AccountResponse : BaseResponse
{
    public Account? Account { get; set; }
}