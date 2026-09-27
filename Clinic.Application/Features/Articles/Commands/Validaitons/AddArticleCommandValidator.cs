using Clinic.Application.Features.Articles.Commands.Models;
using FluentValidation;

namespace Clinic.Application.Features.Articles.Commands.Validaitons
{
    public sealed class AddArticleCommandValidator : AbstractValidator<AddArticleCommand>
    {
        public AddArticleCommandValidator()
        {
            RuleFor(x => x.Title)
    .NotEmpty()
    .WithMessage("Title is required.")
    .MaximumLength(100)
    .WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Summary)
                .NotEmpty()
                .WithMessage("Summary is required.")
                .MinimumLength(100)
                .WithMessage("Summary must be at least 100 characters.")
                .MaximumLength(200)
                .WithMessage("Summary cannot exceed 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MinimumLength(500)
                .WithMessage("Content must be at least 500 characters.")
                .MaximumLength(2000)
                .WithMessage("Content cannot exceed 2000 characters.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid article status.");



        }
    }
}
