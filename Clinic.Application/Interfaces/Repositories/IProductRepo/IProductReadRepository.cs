using BackEnd.Domain.Entities;
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.SearchCriteria;

namespace Clinic.Application.Interfaces.Repositories.ProductRepo
{
    public interface IProductReadRepository : IReadRepository<Product, ProductSearchCriteria, int>
    {
    }
}
