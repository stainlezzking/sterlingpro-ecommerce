using MediatR;

namespace sterlingpro.assessment.Features.Cart.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<string>
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
