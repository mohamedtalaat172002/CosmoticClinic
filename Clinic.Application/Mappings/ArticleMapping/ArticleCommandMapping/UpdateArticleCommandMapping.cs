using AutoMapper;

namespace Clinic.Application.Mappings.ArticleMapping
{
    public partial class ArticleProfile : Profile
    {
        public void UpdateArticleMapping()
        {
            CreateMap<
                Clinic.Domain.Entities.artical.Article,
                Clinic.Application.Features.Articles.Commands.Models.UpdateArticleCommand
            >().ReverseMap();
        }
    }
}
