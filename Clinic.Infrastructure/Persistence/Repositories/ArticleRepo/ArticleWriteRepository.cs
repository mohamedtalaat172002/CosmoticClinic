using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Domain.Entities.artical;
using Clinic.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence.Repositories.ArticleRepo
{
    public class ArticleWriteRepository : GenericRepository<Article, Guid>, IArticleWriteRepository
    {
        public ArticleWriteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Article?> GetWithImagesForUpdateAsync(Guid id, CancellationToken ct)
        {
            return _set.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id, ct);
        }
    }
}
