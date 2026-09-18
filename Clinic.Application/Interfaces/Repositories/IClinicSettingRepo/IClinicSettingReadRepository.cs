using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Domain.Entities.clinicSettings;

namespace Clinic.Application.Interfaces.Repositories.IClinicSettingRepo
{
    public interface IClinicSettingReadRepository : IReadRepository<ClinicSettings, ClinicSettingsSearchCriteria, Guid>
    {
    }
}
