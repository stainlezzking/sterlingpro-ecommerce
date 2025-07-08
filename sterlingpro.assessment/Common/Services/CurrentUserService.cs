using System.Security.Claims;

namespace sterlingpro.assessment.Common.Services
{

    public interface ICurrentUserService
    {
        int UserId { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
    }
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
                return int.TryParse(userIdClaim?.Value, out var userId) ? userId : 0;
            }
        }

        public string Email => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
