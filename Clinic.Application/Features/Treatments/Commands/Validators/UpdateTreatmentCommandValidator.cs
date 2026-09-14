// UpdateTreatmentCommandValidator.cs

using Clinic.Application.Features.Treatments.Commands.Models;
using FluentValidation;

namespace Clinic.Application.Features.Treatments.Commands.Validators
{
    public sealed class UpdateTreatmentCommandValidator
        : AbstractValidator<UpdateTreatmentCommand>
    {
        public UpdateTreatmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl));
        }
    }
}