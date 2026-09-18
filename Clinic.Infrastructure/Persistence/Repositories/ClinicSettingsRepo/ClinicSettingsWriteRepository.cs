using Clinic.Application.Interfaces.Repositories.IClinicSettingRepo;
using Clinic.Domain.Entities.clinicSettings;
using Clinic.Infrastructure.Persistence.DbContext;

namespace Clinic.Infrastructure.Persistence.Repositories.ClinicSettingsRepo
{
    public class ClinicSettingsWriteRepository : GenericRepository<ClinicSettings, Guid>, IClinicSettingWriteRepository
    {
        public ClinicSettingsWriteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
