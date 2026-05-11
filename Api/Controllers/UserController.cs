using Application.Responses;
using Application.Users.LogIn;
using Application.Users.Register;
using Application.Users.UserUpdateEMail;
using Application.Users.UserUpdateName;
using Application.Users.UserUpdatePassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("user/log-in")]
        public async Task<ActionResult<LogInResponse>> LogIn([FromQuery] LogInQuery request, CancellationToken cancellationToken)
        {
            try 
            { 
                
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        [HttpPost("user/register")]
        public async Task<ActionResult> Register([FromBody] RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _sender.Send(request, cancellationToken);
                return Ok("successful");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }
        [Authorize]
        [HttpPatch("user/update-email")]
        public async Task<ActionResult> UpdateEMail([FromBody] UserUpdateEMailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _sender.Send(request, cancellationToken);
                return Ok("successful");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }
        [Authorize]
        [HttpPatch("user/update-name")]
        public async Task<ActionResult> UpdateName([FromBody] UserUpdateNameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _sender.Send(request, cancellationToken);
                return Ok("successful");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }
        [Authorize]
        [HttpPatch("user/update-password")]
        public async Task<ActionResult> UpdatePassword([FromBody] UserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _sender.Send(request, cancellationToken);
                return Ok("successful");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }
    }
}
