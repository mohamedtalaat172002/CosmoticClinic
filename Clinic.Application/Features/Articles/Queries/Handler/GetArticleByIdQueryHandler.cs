using AutoMapper;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using MediatR;

namespace Clinic.Application.Features.Articles.Queries.Handler
{
    internal sealed class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, Result<SingleArticleDto>>
    {
        private readonly IArticleReadRepository _articleRepository;
        private readonly IMapper _mapper;
        public GetArticleByIdQueryHandler(IArticleReadRepository articleRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }
        public async Task<Result<SingleArticleDto>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetById(request.id);

            if (article is null || article.IsDeleted)
                return Result<SingleArticleDto>.Failure("Article not found", ErrorType.NotFound);

            var mappedArticle = _mapper.Map<SingleArticleDto>(article);
            return Result<SingleArticleDto>.Success(mappedArticle);
        }
    }
}
