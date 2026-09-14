using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using Clinic.Domain.Entities.treatment;
using Clinic.Infrastructure.Persistence.DbContext;

namespace Clinic.Infrastructure.Persistence.Repositories.TreatmentRepo
{
    public class TreatmentWriteRepository : GenericRepository<Treatment, Guid>, ITreatmentWriteRepository
    {
        public TreatmentWriteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
