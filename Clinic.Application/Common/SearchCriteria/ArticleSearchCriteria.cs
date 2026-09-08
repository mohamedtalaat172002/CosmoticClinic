using Clinic.Domain.Entities.artical;

namespace Clinic.Application.Common.SearchCriteria
{
    public class ArticleSearchCriteria : BaseSearchCriteria
    {

        public Guid? WriterId { get; set; }
        public ArticleStatus? Status { get; set; }
    }
}
