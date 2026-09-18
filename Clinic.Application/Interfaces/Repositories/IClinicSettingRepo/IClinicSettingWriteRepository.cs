using Clinic.Application.Abstractions.Persistence;
using Clinic.Domain.Entities.clinicSettings;

namespace Clinic.Application.Interfaces.Repositories.IClinicSettingRepo
{
    public interface IClinicSettingWriteRepository : IGenericRepository<ClinicSettings, Guid>
    {
    }
}
