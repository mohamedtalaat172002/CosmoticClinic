using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Application.Features.Treatments.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Treatments.Queries.Models
{
    public sealed class GetAllTreatmentsQuery
      : TreatmentSearchCriteria,
        IRequest<Result<PagedResult<AllTreatmentsDto>>>
    {
    }
}
