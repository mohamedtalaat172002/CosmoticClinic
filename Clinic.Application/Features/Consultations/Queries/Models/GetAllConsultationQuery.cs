using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Application.Features.Consultations.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Consultations.Queries.Models
{
    public sealed class GetAllConsultationQuery : ConsultationSearchCriteria, IRequest<Result<PagedResult<AllConsultationsDto>>>
    {
    }
}
