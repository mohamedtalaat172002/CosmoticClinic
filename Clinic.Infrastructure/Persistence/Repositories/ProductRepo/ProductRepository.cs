using BackEnd.Domain.Entities;
using Clinic.Application.Interfaces.Repositories.ProductRepo;
using Clinic.Infrastructure.Persistence.DbContext;

namespace Clinic.Infrastructure.Persistence.Repositories.ProductRepo
{
    public class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
