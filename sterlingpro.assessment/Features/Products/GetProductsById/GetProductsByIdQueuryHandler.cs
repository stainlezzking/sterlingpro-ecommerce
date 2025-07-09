using MediatR;
using Microsoft.EntityFrameworkCore;
using sterlingpro.assessment.Common.Database;
using sterlingpro.assessment.Common.Models;
using sterlingpro.assessment.Common.Services;

namespace sterlingpro.assessment.Features.Products.GetProductsById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService clI;

        public GetProductByIdQueryHandler(ApplicationDbContext context, ICurrentUserService clI)
        {
            _context = context;
            this.clI = clI;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive, cancellationToken);

            if (product == null)  throw new ArgumentException($"Product with ID {request.Id} not found");

            return product;
        }
    }
}
