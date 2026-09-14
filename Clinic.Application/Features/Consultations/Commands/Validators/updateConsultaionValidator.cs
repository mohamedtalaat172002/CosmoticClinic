using Clinic.Application.Features.Consultations.Commands.Models;
using FluentValidation;

namespace Clinic.Application.Features.Consultations.Commands.Validators
{
    public class updateConsultaionValidator : AbstractValidator<UpdateConsultationCommand>
    {
        public updateConsultaionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Consultation ID is required.");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{9,15}$").WithMessage("Phone number must be valid and contain 9 to 15 digits.");
            RuleFor(x => x.Message)
                .MaximumLength(500).WithMessage("Message cannot exceed 500 characters.");

        }
    }
}
