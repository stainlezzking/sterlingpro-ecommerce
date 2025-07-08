using MediatR;
using Microsoft.AspNetCore.Mvc;
using sterlingpro.assessment.Common.Dto;

namespace sterlingpro.assessment.Features.Authentication.Login
{
    [ApiController]
    [Route("api/auth")]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var command = new LoginCommand
                {
                    Email = loginDto.Email,
                    Password = loginDto.Password
                };

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }
    }
}
