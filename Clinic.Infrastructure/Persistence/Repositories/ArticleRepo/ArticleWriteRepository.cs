using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Domain.Entities.artical;
using Clinic.Infrastructure.Persistence.DbContext;

namespace Clinic.Infrastructure.Persistence.Repositories.ArticleRepo
{
    public class ArticleWriteRepository : GenericRepository<Article, Guid>, IArticleWriteRepository
    {
        public ArticleWriteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
