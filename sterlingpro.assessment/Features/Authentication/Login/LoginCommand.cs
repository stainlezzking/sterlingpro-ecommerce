using MediatR;
using sterlingpro.assessment.Common.Dto;

namespace sterlingpro.assessment.Features.Authentication.Login
{
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
