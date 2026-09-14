using Clinic.Application.Interfaces.Repositories.IConsultationRepo;
using Clinic.Domain.Entities.consultation;
using Clinic.Infrastructure.Persistence.DbContext;

namespace Clinic.Infrastructure.Persistence.Repositories.ConsultationRepo
{
    public class ConsultationWriteRepository : GenericRepository<Consultation, Guid>, IConsultationWriteRepository
    {
        public ConsultationWriteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
