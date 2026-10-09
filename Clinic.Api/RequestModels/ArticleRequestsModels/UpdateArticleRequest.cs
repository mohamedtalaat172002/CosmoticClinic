using Clinic.Domain.Entities.artical;

namespace Clinic.Api.RequestModels.ArticleRequestsModels
{
    public class UpdateArticleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public ArticleStatus Status { get; set; }
        public Guid? TreatmentId { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
