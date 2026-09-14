using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Domain.Entities.treatment;

namespace Clinic.Application.Interfaces.Repositories.ITreatmentRepo
{
    public interface ITreatmentReadRepository : ISlugReadRepository<Treatment, TreatmentSearchCriteria, Guid>
    {
        Task<Treatment?> GetByIdWithArticlesAsync(Guid id);
    }
}
