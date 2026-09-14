using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.SearchCriteria;

namespace Clinic.Application.Interfaces.Repositories.IConsultationRepo
{
    public interface IConsultationReadRepository : IReadRepository<Domain.Entities.consultation.Consultation, ConsultationSearchCriteria, Guid>
    {
    }
}
