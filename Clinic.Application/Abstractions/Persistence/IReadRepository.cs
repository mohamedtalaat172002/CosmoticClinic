using Clinic.Application.Common.SearchCriteria;
using Clinic.Domain.Common;

namespace Clinic.Application.Abstractions.Persistence
{
    public interface IReadRepository<TEntity, TEntitySC, TId>
        where TEntity : BaseEntity<TId>
        where TEntitySC : BaseSearchCriteria
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity?> GetById(TId id);
        IQueryable<TEntity> GetAllBySearchCriteria(
            IQueryable<TEntity> query,
            TEntitySC criteria);
        IQueryable<TEntity> ApplyCustomFilters(
                IQueryable<TEntity> query,
                TEntitySC criteria);
    }
}
