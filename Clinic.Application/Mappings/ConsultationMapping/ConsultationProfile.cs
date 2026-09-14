using AutoMapper;

namespace Clinic.Application.Mappings.ConsultationMapping
{
    public partial class ConsultationProfile : Profile
    {
        public ConsultationProfile()
        {
            GetAllConsultaionsMapping();
            GetSingleConultaionByIdMapping();
            AddConsultaionMapping();
            UpdateConsultaionMapping();
        }
    }
}
