using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XPassword.API.Models.Requests;
using XPassword.Database.Logic;
using XPassword.Database.Model.Exceptions;
using XPassword.Security;

namespace XPassword.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    [HttpPost("LogIn")]
    public ObjectResult RequestLoginToken([FromBody] Models.Requests.LoginRequest loginRequest)
    {
        try
        {
            using var logic = new Accounts();
            var validCredentials = logic.LogIn(loginRequest.Email, loginRequest.Password);

            if (!validCredentials) 
                return Ok("Email and/or password are/is invalid!");

            var token = JwtTokenManager.GenerateJwtToken(loginRequest.Email, loginRequest.Password, 300);

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
    public ObjectResult CreateAccount([FromBody] AccountCreationRequest request)
    {
        try
        {
            using var logic = new Accounts();
            var createdAccount = logic.CreateAccount(request.Username, request.Email, request.Password, request.ConfirmPassword);

            if (!createdAccount)
                return Ok("Unexpected error");

            var token = JwtTokenManager.GenerateJwtToken(request.Email, request.Password, 300);

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
        var (email, password) = JwtTokenManager.ExtractEmailAndPassword(User);

        return Ok(new { message = "This is a protected API endpoint" });
    }
}