using AutoMapper;
using Clinic.Application.Features.ClinicSetting.Queries.Result;
using Clinic.Domain.Entities.clinicSettings;

namespace Clinic.Application.Mappings.ClinicSettingsMapping
{
    public partial class ClinicSettingProfile : Profile
    {
        public void GetClinicSettingQueryMapping()
        {
            CreateMap<ClinicSettings, ClinicSettingDto>();

        }
    }
}
