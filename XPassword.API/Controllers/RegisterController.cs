using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XPassword.API.Models.Requests;
using XPassword.Database.Logic;
using XPassword.Database.Model.Exceptions;
using XPassword.Security;

namespace XPassword.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    [Authorize]
    [HttpPut("AddRegisters")]
    public ObjectResult AddRegisters([FromBody] AddRegisterRequest request)
    {
        try
        {
            var (email, password) = JwtTokenManager.ExtractEmailAndPassword(User);

            using var regLogic = new Registers();

            var registers = Program.Mapper!.Map<List<Database.Model.Register>>(request.RegisterList);
            var total = registers.Count;
            var inserted = regLogic.AddRegisters(registers, email, password);

            return Ok($"{inserted} of {total} inserted with success!");
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
    [HttpGet("GetRegisters")]
    public ObjectResult GetRegisters()
    {
        try
        {
            var (email, password) = JwtTokenManager.ExtractEmailAndPassword(User);

            using var regLogic = new Registers();


            return Ok("");
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
}