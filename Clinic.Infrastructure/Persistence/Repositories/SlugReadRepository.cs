using Clinic.Application.Common.SearchCriteria;
using Clinic.Domain.BaseEntity;
using Clinic.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence.Repositories
{
    public class SlugReadRepository<TEntity, TEntitySC, TId>
       : ReadRepository<TEntity, TEntitySC, TId>
       where TEntity : SluggableBaseEntity<TId>
       where TEntitySC : BaseSearchCriteria
    {
        protected SlugReadRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public virtual async Task<TEntity?> GetBySlugAsync(string slug)
        {
            return await _context.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Slug == slug);
        }
    }
}
