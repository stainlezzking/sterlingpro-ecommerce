using MediatR;
using sterlingpro.assessment.Common.Models;

namespace sterlingpro.assessment.Features.Products.GetProducts
{
    public class GetProductsQuery : IRequest<List<Product>>
    {
        public string Category { get; set; }
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
