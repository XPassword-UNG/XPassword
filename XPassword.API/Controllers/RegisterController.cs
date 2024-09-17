using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XPassword.API.Models;
using XPassword.API.Models.Requests;
using XPassword.API.Models.Response;
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

            return Ok(new MessageResponse() { Message = $"{inserted} of {total} inserted with success!" });
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

            var qr = regLogic.GetRegisters(email, password);

            var registers = Program.Mapper!.Map<List<Register>>(qr);
            var response = new RegistersResponse()
            {
                Registers = registers,
            };

            return Ok(response);
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