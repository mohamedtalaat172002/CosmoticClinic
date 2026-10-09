using Clinic.Domain.BaseEntity;
using Clinic.Domain.Entities.artical;

namespace Clinic.Domain.Entities
{
    public class ArticleImage : ImageBase
    {
        public Guid ArticleId { get; private set; }

        public Article Article { get; private set; } = null!;

        private ArticleImage()
        {
        }

        public ArticleImage(
            Guid articleId,
            string url,
            string publicId,
            int displayOrder = 0,
            bool isPrimary = false)
            : base(
                url,
                publicId,
                displayOrder,
                isPrimary)
        {
            ArticleId = articleId;
        }
    }
}

