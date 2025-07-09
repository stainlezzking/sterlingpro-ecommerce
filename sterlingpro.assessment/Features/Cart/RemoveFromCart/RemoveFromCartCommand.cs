using MediatR;

namespace sterlingpro.assessment.Features.Cart.RemoveFromCart
{
    public class RemoveFromCartCommand : IRequest<string>
    {
        public int CartItemId { get; set; }
    }
}
