using Clinic.Application.Abstractions.Persistence;

namespace Clinic.Application.Interfaces.Repositories.IConsultationRepo
{
    public interface IConsultationWriteRepository : IGenericRepository<Domain.Entities.consultation.Consultation, Guid>
    {
    }
}
