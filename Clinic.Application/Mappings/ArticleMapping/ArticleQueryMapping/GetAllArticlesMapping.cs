using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Domain.Entities.artical;

namespace Clinic.Application.Mappings.ArticleMapping
{
    public partial class ArticleProfile
    {
        public void GetAllArticlesMapping()
        {
            CreateMap<Article, AllArticlesDto>().ForMember(dest => dest.FeaturedImageUrl, opt => opt.MapFrom(src => src.Images.Where(i => i.IsPrimary).Select(i => new ArticleImageDto { Id = i.Id, Url = i.Url, IsPrimary = i.IsPrimary, DisplayOrder = i.DisplayOrder }).FirstOrDefault()));
        }
    }
}
