using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Interfaces.Repositories.ProductRepo;
using Clinic.Application.Interfaces.Services;
using Clinic.Infrastructure.Persistence.DbContext;
using Clinic.Infrastructure.Persistence.Repositories.ProductRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IProductReadRepository? ProductReadRepository { get; private set; }
        public IProductRepository? ProductRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            this.ProductReadRepository = new ProductReadRepository(context);
            this.ProductRepository = new ProductRepository(context);


        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
