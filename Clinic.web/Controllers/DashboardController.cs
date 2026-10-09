using Clinic.Application.Features.ClinicSetting.Commands.Models;
using Clinic.Application.Features.ClinicSetting.Queries.Models;
using Clinic.web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

[Authorize]
public sealed class DashboardController(IMediator mediator) : Controller
{

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetClinicSettingsQueryModel(), cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return View(null);

        return View(result.Value);
    }



    [HttpGet]
    public async Task<IActionResult> Settings(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetClinicSettingsQueryModel(), cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return View(new ClinicSettingsInputModel());


        var model = new ClinicSettingsInputModel
        {
            ClinicName = result.Value.ClinicName,
            DoctorName = result.Value.DoctorName,
            Specialization = result.Value.Specialization,
            Description = result.Value.Description,
            WhatsAppNumber = result.Value.WhatsAppNumber,
            Email = result.Value.Email,
            Address = result.Value.Address,
            FacebookUrl = result.Value.FacebookUrl,
            InstagramUrl = result.Value.InstagramUrl,
            ExistingImages = result.Value.Images.ToList()
        };

        return View(model);
    }

    // PUT /Dashboard/Settings
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(
        ClinicSettingsInputModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var uploads = model.Images
            .Where(image => image.Length > 0)
            .Select(image => new ClinicSettingImageUpload(image.OpenReadStream(), image.FileName))
            .ToList();

        var command = new UpdateClinicSettingModel(
            model.ClinicName,
            model.DoctorName,
            model.Specialization,
            model.Description,
            model.WhatsAppNumber,
            model.Email,
            model.Address,
            model.FacebookUrl,
            model.InstagramUrl,
            uploads);

        var result = await mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Clinic settings saved successfully.";
            return RedirectToAction(nameof(Settings));
        }

        ModelState.AddModelError(string.Empty, result.Message ?? "Could not save settings.");
        return View(model);
    }
}
