using Clinic.Application.Common.SearchCriteria;
using Clinic.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Abstractions.Persistence
{
    public interface IEntityStateRepository<TEntity, TId>
        where TEntity : BaseEntity<TId>
    {
        Task ActivateAsync(TId id);
        Task DeactivateAsync(TId id);
        Task SoftDeleteAsync(TId id);
        Task RestoreAsync(TId id);
        Task<int> CountAsync(BaseSearchCriteria criteria);
        IQueryable<TEntity> ApplyCountCustomFilters(
               IQueryable<TEntity> query,
               BaseSearchCriteria criteria);
    }
}
