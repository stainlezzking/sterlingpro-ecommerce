using MediatR;
using Microsoft.EntityFrameworkCore;
using sterlingpro.assessment.Common.Database;
using sterlingpro.assessment.Common.Services;

namespace sterlingpro.assessment.Features.Cart.RemoveFromCart
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, string>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public RemoveFromCartCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            // Get cart item
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == request.CartItemId && ci.Cart.UserId == userId, cancellationToken);

            if (cartItem == null)
            {
                throw new ArgumentException("Cart item not found");
            }

            // Remove item from cart
            _context.CartItems.Remove(cartItem);

            // Update cart timestamp
            cartItem.Cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return "Item removed from cart successfully";
        }
    }
}
