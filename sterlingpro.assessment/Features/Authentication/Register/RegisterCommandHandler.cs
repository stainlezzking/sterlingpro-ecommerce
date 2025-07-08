using MediatR;
using Microsoft.EntityFrameworkCore;
using sterlingpro.assessment.Common.Database;
using sterlingpro.assessment.Common.Dto;
using sterlingpro.assessment.Common.Models;
using sterlingpro.assessment.Common.Services;
using BCrypt.Net;

namespace sterlingpro.assessment.Features.Authentication.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public RegisterCommandHandler(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            // Create new user
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            // Generate token
            var token = _jwtService.GenerateToken(user);
            var tokenExpiration = _jwtService.GetTokenExpiration();

            return new AuthResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = token,
                TokenExpiration = tokenExpiration
            };
        }
    }
}
