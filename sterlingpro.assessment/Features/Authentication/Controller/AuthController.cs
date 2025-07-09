using MediatR;
using Microsoft.AspNetCore.Mvc;
using sterlingpro.assessment.Common.Dto;
using sterlingpro.assessment.Features.Authentication.Login;
using sterlingpro.assessment.Features.Authentication.Register;

namespace sterlingpro.assessment.Features.Authentication.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
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
