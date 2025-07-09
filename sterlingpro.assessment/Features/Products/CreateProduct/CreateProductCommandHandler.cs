using MediatR;
using sterlingpro.assessment.Common.Database;
using sterlingpro.assessment.Common.Models;

namespace sterlingpro.assessment.Features.Products.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly ApplicationDbContext _context;

        public CreateProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name)) throw new ArgumentException("Product name is required");
            
            if (request.Price <= 0) throw new ArgumentException("Price must be greater than 0");

            if (request.StockQuantity < 0) throw new ArgumentException("Stock quantity cannot be negative");
            
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Category = request.Category ?? "General",
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product;
        }
    }
}
