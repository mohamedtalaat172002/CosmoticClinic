using BackEnd.Domain.Entities;
using Clinic.Application.Abstractions.Persistence;

namespace Clinic.Application.Interfaces.Repositories.ProductRepo
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
    }
}
