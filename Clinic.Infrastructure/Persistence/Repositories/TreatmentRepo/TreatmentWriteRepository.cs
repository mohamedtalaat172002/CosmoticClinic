using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using Clinic.Domain.Entities.treatment;
using Clinic.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence.Repositories.TreatmentRepo
{
    public class TreatmentWriteRepository : GenericRepository<Treatment, Guid>, ITreatmentWriteRepository
    {
        public TreatmentWriteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Treatment?> GetWithImagesForUpdateAsync(Guid id, CancellationToken cancellationToken)
        => _set.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    }
}
