using Clinic.Application.Interfaces.Services;
using System.Text.RegularExpressions;

namespace Clinic.Infrastructure.Services.ArticleService
{
    public sealed class SlugService : ISlugService
    {


        public string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be empty.", nameof(text));

            text = text.Trim().ToLowerInvariant();

            text = Regex.Replace(text, @"\s+", "-");

            text = Regex.Replace(text, @"[^a-z0-9\-]", "");

            text = Regex.Replace(text, @"-+", "-");

            return text.Trim('-');
        }

    }
}
