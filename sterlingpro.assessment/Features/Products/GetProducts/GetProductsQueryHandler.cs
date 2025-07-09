using MediatR;
using Microsoft.EntityFrameworkCore;
using sterlingpro.assessment.Common.Database;
using sterlingpro.assessment.Common.Models;

namespace sterlingpro.assessment.Features.Products.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>
    {
        private readonly ApplicationDbContext _context;

        public GetProductsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(request.Category))
            {
                query = query.Where(p => p.Category.ToLower() == request.Category.ToLower());
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(request.SearchTerm) ||
                                        p.Description.Contains(request.SearchTerm));
            }

            return await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }
    }
}
