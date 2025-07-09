using MediatR;
using sterlingpro.assessment.Common.Models;

namespace sterlingpro.assessment.Features.Products.GetProductsById
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }
}
