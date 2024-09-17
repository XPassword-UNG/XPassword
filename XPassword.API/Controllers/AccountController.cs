using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XPassword.API.Models.Requests;
using XPassword.API.Models.Response;
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
                return Ok(new BaseResponse() { Success = false, Error = "Dados inválidos" });

            var token = JwtTokenManager.GenerateJwtToken(loginRequest.Email, loginRequest.Password, 300);

            return Ok(new TokenResponse() { Token = token, LifeTime = 600 });
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
                return Ok(new BaseResponse() { Success = false, Error = "Unexpected Error" });

            var token = JwtTokenManager.GenerateJwtToken(request.Email, request.Password, 300);

            return Ok(new TokenResponse() { Token = token, LifeTime = 600 });
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
        var q = 5;

        return Ok(new MessageResponse() { Message = $"Sua conta foi deletada junto dos seus {q} registros" });
    }
}