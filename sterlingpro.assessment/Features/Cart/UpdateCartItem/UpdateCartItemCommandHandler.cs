using MediatR;
using Microsoft.EntityFrameworkCore;
using sterlingpro.assessment.Common.Database;
using sterlingpro.assessment.Common.Services;

namespace sterlingpro.assessment.Features.Cart.UpdateCartItem
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, string>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateCartItemCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            // Get cart item with product info
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == request.CartItemId && ci.Cart.UserId == userId, cancellationToken);

            if (cartItem == null)
            {
                throw new ArgumentException("Cart item not found");
            }

            // Check stock availability
            if (cartItem.Product.StockQuantity < request.Quantity)
            {
                throw new InvalidOperationException("Insufficient stock");
            }

            // Update quantity
            cartItem.Quantity = request.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;

            // Update cart timestamp
            cartItem.Cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return "Cart item updated successfully";
        }
    }
}
