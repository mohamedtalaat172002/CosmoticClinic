using Clinic.Application.Abstractions.Persistence;
using Clinic.Domain.Entities.artical;

namespace Clinic.Application.Interfaces.Repositories.IArticalRepo
{
    public interface IArticleWriteRepository : IGenericRepository<Article, Guid>
    {

    }
}
