namespace Clinic.Domain.BaseEntity
{
    public abstract class ImageBase
    {
        public Guid Id { get; private set; }

        public string Url { get; private set; } = string.Empty;

        public string PublicId { get; private set; } = string.Empty;

        public int DisplayOrder { get; private set; }

        public bool IsPrimary { get; private set; }

        protected ImageBase()
        {
        }

        protected ImageBase(
            string url,
            string publicId,
            int displayOrder = 0,
            bool isPrimary = false)
        {
            Id = Guid.NewGuid();
            Url = url;
            PublicId = publicId;
            DisplayOrder = displayOrder;
            IsPrimary = isPrimary;
        }
    }
}
