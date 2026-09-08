using Clinic.Application.Interfaces.Repositories.ProductRepo;

namespace Clinic.Application.Abstractions.Persistence
{
    public interface IUnitOfWork
    {
        public IProductReadRepository ProductReadRepository { get; }
        public IProductRepository ProductRepository { get; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
