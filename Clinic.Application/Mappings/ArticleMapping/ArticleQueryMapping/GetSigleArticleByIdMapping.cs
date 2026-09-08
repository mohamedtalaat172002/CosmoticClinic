using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Domain.Entities.artical;

namespace Clinic.Application.Mappings.ArticleMapping
{
    public partial class ArticleProfile
    {
        public void GetSingleArticleByIDMapping()
        {
            CreateMap<Article, SingleArticleDto>();
        }


    }
}
