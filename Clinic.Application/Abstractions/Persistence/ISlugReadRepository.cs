using Clinic.Application.Common.SearchCriteria;
using Clinic.Domain.BaseEntity;

namespace Clinic.Application.Abstractions.Persistence
{
    public interface ISlugReadRepository<TEntity, TEntitySC, TId>
       : IReadRepository<TEntity, TEntitySC, TId>
       where TEntity : SluggableBaseEntity<TId>
       where TEntitySC : BaseSearchCriteria
    {
        Task<TEntity?> GetBySlugAsync(string slug);
    }
}
