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
        private readonly ApplicationDbContext _context;

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

        public virtual IQueryable<TEntity> GetAllBySearchCriteria(
            IQueryable<TEntity> query,
            TEntitySC criteria)
        {
            var pageIndex = criteria.PageIndex ?? 1;
            var pageSize = criteria.PageSize ?? 10;

            //if (pageSize > criteria.MaxPageSize)
            //    pageSize = criteria.MaxPageSize;


            query = query.WhereIf(criteria.IsDeleted.HasValue, e => e.IsDeleted == criteria.IsDeleted);

            query = ApplyCustomFilters(query, criteria);

            int skip = (pageIndex - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

            return query;
        }

        public virtual IQueryable<TEntity> ApplyCustomFilters(
                IQueryable<TEntity> query,
                TEntitySC criteria)
        {
            return query;
        }
    }
}
