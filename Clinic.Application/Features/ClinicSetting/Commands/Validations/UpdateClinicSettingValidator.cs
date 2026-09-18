using Clinic.Application.Features.ClinicSetting.Commands.Models;
using FluentValidation;

namespace Clinic.Application.Features.ClinicSetting.Commands.Validations
{
    public sealed class UpdateClinicSettingValidator : AbstractValidator<UpdateClinicSettingModel>
    {
        public UpdateClinicSettingValidator()
        {
            RuleFor(x => x.ClinicName)
                .MaximumLength(150);

            RuleFor(x => x.DoctorName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Specialization)
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.WhatsAppNumber)
                .MaximumLength(30);

            RuleFor(x => x.Email)
                .MaximumLength(255)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.LogoUrl)
                .MaximumLength(500);

            RuleFor(x => x.HeroImageUrl)
                .MaximumLength(500);

            RuleFor(x => x.FacebookUrl)
                .MaximumLength(500);

            RuleFor(x => x.InstagramUrl)
                .MaximumLength(500);
        }
    }
}
