using Clinic.Application.Abstractions.Persistence;
using Clinic.Domain.Entities.treatment;

namespace Clinic.Application.Interfaces.Repositories.ITreatmentRepo
{
    public interface ITreatmentWriteRepository : IGenericRepository<Treatment, Guid>
    {
        Task<Treatment?> GetWithImagesForUpdateAsync(Guid id, CancellationToken cancellationToken);
    }
}
