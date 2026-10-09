using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.Extensions;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Domain.Common;
using Clinic.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence.Repositories
{
    public class ReadRepository<TEntity, TEntitySC, TId> : IReadRepository<TEntity, TEntitySC, TId>
        where TEntity : BaseEntity<TId>
        where TEntitySC : BaseSearchCriteria
    {
        protected readonly ApplicationDbContext _context;

        public ReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public virtual Task<TEntity?> GetById(TId id)
        {
            return _context.Set<TEntity>()
                       .AsNoTracking()
                       .FirstOrDefaultAsync(x => x.Id!.Equals(id));
        }

        public virtual IQueryable<TEntity> ApplyFilters(
            IQueryable<TEntity> query,
            TEntitySC criteria)
        {
            query = query.WhereIf(criteria.IsDeleted.HasValue, e => e.IsDeleted == criteria.IsDeleted);

            return ApplyCustomFilters(query, criteria);
        }

        public virtual IQueryable<TEntity> ApplyPaging(
            IQueryable<TEntity> query,
            TEntitySC criteria)
        {
            var skip = (criteria.NormalizedPageIndex - 1) * criteria.NormalizedPageSize;

            return query.Skip(skip).Take(criteria.NormalizedPageSize);
        }

        public virtual IQueryable<TEntity> GetAllBySearchCriteria(
            IQueryable<TEntity> query,
            TEntitySC criteria)
        {
            return ApplyPaging(ApplyFilters(query, criteria), criteria);
        }

        public virtual IQueryable<TEntity> ApplyCustomFilters(
            IQueryable<TEntity> query,
            TEntitySC criteria)
        {
            return query;
        }
    }
}