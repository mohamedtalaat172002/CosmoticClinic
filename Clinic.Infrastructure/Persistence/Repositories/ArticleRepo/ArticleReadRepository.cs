using Clinic.Application.Common.Extensions;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Domain.Entities.artical;
using Clinic.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence.Repositories.ArticleRepo
{
    public class ArticleReadRepository : ReadRepository<Article, ArticleSearchCriteria, Guid>
        , IArticleReadRepository
    {
        public ArticleReadRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IQueryable<Article> ApplyCustomFilters(
    IQueryable<Article> query,
    ArticleSearchCriteria criteria)
        {
            var search = criteria.Search?.Trim();

            return query
                .WhereIf(!string.IsNullOrWhiteSpace(search),
                    a => EF.Functions.Like(a.Title, $"%{search}%") ||
                         EF.Functions.Like(a.Summary, $"%{search}%") ||
                         EF.Functions.Like(a.Content, $"%{search}%"))

                //.WhereIf(criteria.WriterId.HasValue,
                //    a => a. == criteria.WriterId.Value)

                .WhereIf(criteria.Status.HasValue,
                    a => a.Status == criteria.Status.Value);
        }

        public async Task<Article?> GetBySlugAsync(string slug)
        {

            return await _context.Set<Article>()
                      .AsNoTracking()
                      .FirstOrDefaultAsync(a => a.Slug == slug);
        }

    }
}
