using MediatR;
using Microsoft.AspNetCore.Mvc;
using sterlingpro.assessment.Common.Dto;

namespace sterlingpro.assessment.Features.Authentication.Register
{
    [ApiController]
    [Route("api/auth")]
    public class RegisterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegisterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var command = new RegisterCommand
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                    ConfirmPassword = registerDto.ConfirmPassword
                };

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }
    }
}
