using AutoMapper;
using Clinic.Application.Features.ClinicSetting.Commands.Models;
using Clinic.Domain.Entities.clinicSettings;

namespace Clinic.Application.Mappings.ClinicSettingsMapping
{
    public partial class ClinicSettingProfile : Profile
    {
        public void UpdateClinicSettingCommandMapping()
        {
            CreateMap<UpdateClinicSettingModel, ClinicSettings>().ReverseMap();
        }
    }
}
