using Clinic.Application.Interfaces.Repositories.IClinicSettingRepo;
using Clinic.Domain.Entities.clinicSettings;
using Clinic.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence.Repositories.ClinicSettingsRepo
{
    public class ClinicSettingsWriteRepository : GenericRepository<ClinicSettings, Guid>, IClinicSettingWriteRepository
    {
        public ClinicSettingsWriteRepository(ApplicationDbContext context) : base(context)
        {
        }
        public Task<ClinicSettings?> GetWithImagesForUpdateAsync(Guid id, CancellationToken ct) =>
    _set.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
