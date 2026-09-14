using AutoMapper;

namespace Clinic.Application.Mappings.TreatmentMapping
{
    public partial class TreatmentProfile : Profile
    {
        public TreatmentProfile()
        {
            AddTreatmentMapping();
            UpdateTreatmentMapping();
            GetAllTreatmentsMapping();
            GetSingleTreatmentMapping();
        }
    }
}
