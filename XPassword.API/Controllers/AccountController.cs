using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XPassword.Database.Logic;
using XPassword.Database.Model.Exceptions;
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
        catch (Exception e)
        {
            var validation = new ValidationException(e, e.Message);
            return BadRequest(validation.ErrosList);
        }
    }

    [HttpPut("SignIn")]
    public ObjectResult CreateAccount([FromBody] string username, [FromBody] string email, [FromBody] string password, [FromBody] string confirmedPassword)
    {
        try
        {
            using var logic = new Accounts();
            var createdAccount = logic.CreateAccount(username, email, password, confirmedPassword);

            if (!createdAccount)
                Ok("Unexpected error");

            var token = JwtTokenManager.GenerateJwtToken(email, 300);

            return Ok(new { accessToken = token, expiresIn = 300 });
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.ErrosList);
        }
        catch (Exception e)
        {
            var validation = new ValidationException(e, e.Message);
            return BadRequest(validation.ErrosList);
        }
    }

    [Authorize]
    [HttpDelete("DeleteAccount")]
    public ObjectResult DeteleAccount()
    {
        return Ok("Tested");
    }
}