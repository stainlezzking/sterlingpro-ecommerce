using MediatR;

namespace sterlingpro.assessment.Features.Cart.AddToCart
{
    public class AddToCartCommand : IRequest<string>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
