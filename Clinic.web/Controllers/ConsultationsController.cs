using Clinic.web.Models;
using Clinic.Application.Features.Consultations.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

public sealed class ConsultationsController(IMediator mediator) : Controller
{
    [HttpGet]
    public IActionResult Create() => View(new ConsultationInputModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ConsultationInputModel input, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(input);

        var result = await mediator.Send(new AddConsultationCommand(input.FullName, input.PhoneNumber, input.Message), cancellationToken);
        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Your consultation request has been sent. We will contact you shortly.";
            return RedirectToAction(nameof(Create));
        }

        ModelState.AddModelError(string.Empty, result.Message ?? "We could not send your request. Please try again.");
        return View(input);
    }
}
