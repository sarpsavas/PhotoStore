using Application.Users.LogIn;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("user/log-in")]
        public async Task<IActionResult> LogIn([FromBody] LogInQuery request, CancellationToken cancellationToken)
        {
            try 
            { 
            
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
