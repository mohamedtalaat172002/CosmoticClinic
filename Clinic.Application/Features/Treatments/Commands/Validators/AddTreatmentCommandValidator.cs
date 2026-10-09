
using Clinic.Application.Features.Treatments.Commands.Models;
using FluentValidation;

namespace Clinic.Application.Features.Treatments.Commands.Validators
{
    public sealed class AddTreatmentCommandValidator
        : AbstractValidator<AddTreatmentCommand>
    {
        private static readonly string[] AllowedExtensions =
        [
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        ];

        public AddTreatmentCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Treatment name is required.")
                .MaximumLength(100)
                .WithMessage("Treatment name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Treatment description is required.")
                .MaximumLength(3000)
                .WithMessage("Treatment description must not exceed 3000 characters.");

            RuleFor(x => x.Images)
                .NotNull()
                .WithMessage("Treatment images are required.")
                .Must(images => images.Count > 0)
                .WithMessage("At least one treatment image is required.");

            RuleForEach(x => x.Images)
                .ChildRules(image =>
                {
                    image.RuleFor(x => x.FileName)
                        .NotEmpty()
                        .WithMessage("Image file name is required.")
                        .Must(IsAllowedExtension)
                        .WithMessage(
                            "Only JPG, JPEG, PNG, and WEBP images are allowed.");
                });
        }

        private static bool IsAllowedExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            return AllowedExtensions.Contains(
                extension,
                StringComparer.OrdinalIgnoreCase);
        }
    }
}

