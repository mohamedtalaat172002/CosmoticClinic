using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Application.Features.Treatments.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Treatments.Queries.Models
{
    public sealed class GetTreatmentBySlugQuery : ArticleSearchCriteria,
        IRequest<Result<SingleTreatmentDto>>
    {
        public string Slug { get; set; } = string.Empty;
    }
}
