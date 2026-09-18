using Clinic.Application.Common.SearchCriteria;
using Clinic.Application.Interfaces.Repositories.IClinicSettingRepo;
using Clinic.Domain.Entities.clinicSettings;
using Clinic.Infrastructure.Persistence.DbContext;

namespace Clinic.Infrastructure.Persistence.Repositories.ClinicSettingsRepo
{
    public class ClinicSettingsReadRepository : ReadRepository<ClinicSettings, ClinicSettingsSearchCriteria, Guid>
        , IClinicSettingReadRepository
    {
        public ClinicSettingsReadRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
