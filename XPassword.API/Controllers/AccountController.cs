using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XPassword.API.Models;
using XPassword.Security;

namespace XPassword.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    public AccountController() { }

    [HttpPost("Login")]
    public ObjectResult RequestLoginToken([FromBody] Models.Requests.LoginRequest loginRequest)
    {
        try
        {
            var token = JwtTokenManager.GenerateJwtToken(loginRequest.Email, 300);

            return Ok(new {  accessToken = token, expiresIn = 300 });
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.ErrosList);
        }
    }

    [Authorize]
    [HttpDelete("DeleteAccount")]
    public ObjectResult DeteleAccount()
    {
        return Ok("Tested");
    }
}