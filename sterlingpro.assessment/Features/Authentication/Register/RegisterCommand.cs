using MediatR;
using sterlingpro.assessment.Common.Dto;

namespace sterlingpro.assessment.Features.Authentication.Register
{
    public class RegisterCommand : IRequest<AuthResponseDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
