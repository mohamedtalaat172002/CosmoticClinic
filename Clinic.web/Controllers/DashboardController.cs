using Clinic.Application.Features.ClinicSetting.Commands.Models;
using Clinic.Application.Features.ClinicSetting.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

[Authorize]
public sealed class DashboardController(IMediator mediator) : Controller
{
    // GET /Dashboard
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetClinicSettingsQueryModel(), cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return View(null);   // view handles null gracefully

        return View(result.Value);
    }

    // GET /Dashboard/Settings
    [HttpGet]
    public async Task<IActionResult> Settings(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetClinicSettingsQueryModel(), cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return View(new UpdateClinicSettingModel());

        // Pre-populate the form from the current settings DTO
        var model = new UpdateClinicSettingModel
        {
            ClinicName     = result.Value.ClinicName,
            DoctorName     = result.Value.DoctorName,
            Specialization = result.Value.Specialization,
            Description    = result.Value.Description,
            WhatsAppNumber = result.Value.WhatsAppNumber,
            Email          = result.Value.Email,
            Address        = result.Value.Address,
            LogoUrl        = result.Value.LogoUrl,
            HeroImageUrl   = result.Value.HeroImageUrl,
            FacebookUrl    = result.Value.FacebookUrl,
            InstagramUrl   = result.Value.InstagramUrl,
        };

        return View(model);
    }

    // POST /Dashboard/Settings
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(
        UpdateClinicSettingModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await mediator.Send(model, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Clinic settings saved successfully.";
            return RedirectToAction(nameof(Settings));
        }

        ModelState.AddModelError(string.Empty, result.Message ?? "Could not save settings.");
        return View(model);
    }
}
