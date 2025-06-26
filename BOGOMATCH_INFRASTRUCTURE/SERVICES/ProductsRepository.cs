using BOGOGMATCH_DOMAIN.INTERFACE;
using BOGOGMATCH_DOMAIN.MODELS.UserManagement;
using BOGOMATCH_DOMAIN.INTERFACE;
using BOGOMATCH_DOMAIN.MODELS.Product;
using BOGOMATCH_INFRASTRUCTURE.DATABASE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BOGOMATCH_INFRASTRUCTURE.SERVICES
{
    public class ProductsRepository : IProducts
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public ProductsRepository(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                var products = await _context.Products
    .AsNoTracking() // Prevents EF Core from tracking changes
    .Include(p => p.Reviews)
    .ToListAsync();
                return products;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
