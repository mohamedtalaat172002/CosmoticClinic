using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Domain.Entities.artical;

namespace Clinic.Application.Interfaces.Repositories.IArticalRepo
{
    public interface IArticleReadRepository : IReadRepository<Article, ArticleSearchCriteria, Guid>
    {
        public Task<Article?> GetBySlugAsync(string slug);
    }
}
