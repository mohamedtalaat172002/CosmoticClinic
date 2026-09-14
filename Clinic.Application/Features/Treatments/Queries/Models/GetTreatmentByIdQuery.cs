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
//'https://localhost:7057/Api/V1/Treatment/45b20df4-13ac-4623-b01c-46579bcfccd6?Id=45b20df4-13ac-4623-b01c-46579bcfccd6' \
