using MediatR;
using sterlingpro.assessment.Common.Models;

namespace sterlingpro.assessment.Features.Products.GetProducts
{
    public class GetProductsQuery : IRequest<List<Product>>
    {
   
    }
}
