using MediatR;
using sterlingpro.assessment.Common.Models;

namespace sterlingpro.assessment.Features.Products.CreateProduct
{
    public class CreateProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
