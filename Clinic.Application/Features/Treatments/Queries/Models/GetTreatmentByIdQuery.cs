using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Application.Features.Treatments.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Treatments.Queries.Models
{
    public sealed class GetTreatmentByIdQuery()
       : ArticleSearchCriteria,
         IRequest<Result<SingleTreatmentDto>>
    {

        public Guid Id;
    }
}
