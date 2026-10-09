using Clinic.Application.Features.Articles.Commands.Models;
using FluentValidation;

namespace Clinic.Application.Features.Articles.Commands.Validaitons
{
    public sealed class AddArticleCommandValidator : AbstractValidator<AddArticleCommand>
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

        public AddArticleCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Summary)
                .NotEmpty().WithMessage("Summary is required.")
                .MinimumLength(100).WithMessage("Summary must be at least 100 characters.")
                .MaximumLength(200).WithMessage("Summary cannot exceed 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(500).WithMessage("Content must be at least 500 characters.")
                .MaximumLength(3000).WithMessage("Content cannot exceed 3000 characters.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid article status.");

            RuleFor(x => x.Images)
                .NotNull().WithMessage("At least one article image is required.")
                .Must(images => images.Count > 0).WithMessage("At least one article image is required.");

            RuleForEach(x => x.Images).ChildRules(image =>
            {
                image.RuleFor(x => x.FileName)
                    .NotEmpty().WithMessage("Image file name is required.")
                    .Must(IsAllowedExtension).WithMessage("Only JPG, JPEG, PNG, and WEBP images are allowed.");
            });
        }

        private static bool IsAllowedExtension(string fileName) =>
            AllowedExtensions.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase);
    }
}
