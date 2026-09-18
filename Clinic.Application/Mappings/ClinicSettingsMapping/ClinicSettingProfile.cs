using AutoMapper;

namespace Clinic.Application.Mappings.ClinicSettingsMapping
{
    public partial class ClinicSettingProfile : Profile
    {
        public ClinicSettingProfile()
        {
            GetClinicSettingQueryMapping();
            UpdateClinicSettingCommandMapping();
        }
    }
}
