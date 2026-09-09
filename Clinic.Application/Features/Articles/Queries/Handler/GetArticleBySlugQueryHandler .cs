using AutoMapper;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Application.Interfaces.Services;
using MediatR;

namespace Clinic.Application.Features.Articles.Queries.Handler
{
    internal sealed class GetArticleBySlugQueryHandler
        : IRequestHandler<GetArticleBySlugQuery, Result<SingleArticleDto>>
    {
        private readonly IArticleReadRepository _articleRepository;
        private readonly IMapper _mapper;

        public GetArticleBySlugQueryHandler(
            IArticleReadRepository articleRepository,
            IMapper mapper,
            ISlugService slugService)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;

        }

        public async Task<Result<SingleArticleDto>> Handle(
            GetArticleBySlugQuery request,
            CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetBySlugAsync(request.Slug);

            if (article is null || article.IsDeleted)
                return Result<SingleArticleDto>.Failure(
                    "Article not found",
                    ErrorType.NotFound);

            var mappedArticle = _mapper.Map<SingleArticleDto>(article);

            return Result<SingleArticleDto>.Success(mappedArticle);
        }
    }
}
