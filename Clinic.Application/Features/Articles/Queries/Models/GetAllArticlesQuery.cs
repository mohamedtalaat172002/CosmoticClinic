using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Application.Features.Articles.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.Articles.Queries.Models
{
    public sealed class GetAllArticlesQuery : ArticleSearchCriteria, IRequest<Result<PagedResult<AllArticlesDto>>>
    {

    }
}
