using AutoMapper;

namespace Clinic.Application.Mappings.ArticleMapping
{
    public partial class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            GetSingleArticleByIDMapping();
            GetAllArticlesMapping();
            // AddArticleMapping and UpdateArticleMapping removed — handlers build entities manually
        }
    }
}
