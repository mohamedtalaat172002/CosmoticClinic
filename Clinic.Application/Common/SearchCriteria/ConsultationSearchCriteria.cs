using Clinic.Domain.Entities.consultation;

namespace Clinic.Application.Common.SearchCriteria
{
    public class ConsultationSearchCriteria : BaseSearchCriteria
    {
        public ConsultationStatus? Status { get; set; }
    }
}
