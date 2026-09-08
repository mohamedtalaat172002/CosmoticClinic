using Clinic.Application.Abstractions.Persistence;
using Clinic.Domain.Common;
using Clinic.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _set;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _set = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await _set.FirstOrDefaultAsync(e => e.Id!.Equals(id));
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _set.AddAsync(entity);
        }

        public virtual void Update(TEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _set.Update(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            entity.IsDeleted = true;
            _set.Update(entity);
        }
    }
}
