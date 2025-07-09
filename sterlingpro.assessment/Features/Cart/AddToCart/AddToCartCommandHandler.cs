using MediatR;
using Microsoft.EntityFrameworkCore;
using sterlingpro.assessment.Common.Database;
using sterlingpro.assessment.Common.Models;
using sterlingpro.assessment.Common.Services;

namespace sterlingpro.assessment.Features.Cart.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, string>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AddToCartCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId && p.IsActive, cancellationToken);

            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            // Check stock availability
            if (product.StockQuantity < request.Quantity)
            {
                throw new InvalidOperationException("Insufficient stock");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
            {
                cart = new Common.Models.Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync(cancellationToken);
            }

            // Check if item already exists in cart
            var existingCartItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == request.ProductId);

            if (existingCartItem != null)
            {
                // Update quantity
                existingCartItem.Quantity += request.Quantity;
                existingCartItem.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                // Add new item
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    UnitPrice = product.Price,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.CartItems.Add(cartItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return "Product added to cart successfully";
        }
    }
}
