using MediatR;
using Microsoft.EntityFrameworkCore;
using sterlingpro.assessment.Common.Database;
using sterlingpro.assessment.Common.Services;

namespace sterlingpro.assessment.Features.Cart.GetCart
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartResponseDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetCartQueryHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CartResponseDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
            {
                return new CartResponseDto
                {
                    CartId = 0,
                    Items = new List<CartItemDto>(),
                    TotalAmount = 0,
                    TotalItems = 0,
                    UpdatedAt = DateTime.UtcNow
                };
            }

            var cartItems = cart.CartItems.Select(ci => new CartItemDto
            {
                CartItemId = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                ProductDescription = ci.Product.Description,
                UnitPrice = ci.UnitPrice,
                Quantity = ci.Quantity,
                TotalPrice = ci.UnitPrice * ci.Quantity,
                AvailableStock = ci.Product.StockQuantity
            }).ToList();

            return new CartResponseDto
            {
                CartId = cart.Id,
                Items = cartItems,
                TotalAmount = cartItems.Sum(item => item.TotalPrice),
                TotalItems = cartItems.Sum(item => item.Quantity),
                UpdatedAt = cart.UpdatedAt
            };
        }
    }
}
