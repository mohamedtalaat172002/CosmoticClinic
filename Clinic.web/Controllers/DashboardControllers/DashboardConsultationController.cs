
using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Features.Consultations.Commands.Models;
using Clinic.Application.Features.Consultations.Queries.Models;
using Clinic.Application.Features.Consultations.Queries.Result;
using Clinic.Domain.Entities.consultation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

[Authorize(Roles = "Admin")]
[Route("Dashboard/Consultations")]
public sealed class DashboardConsultationsController(IMediator mediator)
    : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(
        string? search = null,
        ConsultationStatus? status = null,
        int pageIndex = 1,
        int pageSize = 9,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetAllConsultationQuery
            {
                Search = search,
                Status = status,
                PageIndex = pageIndex,
                PageSize = pageSize
            },
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
        {
            return View(
                new PagedResult<AllConsultationsDto>(
                    [],
                    0,
                    pageIndex,
                    pageSize));
        }

        return View(result.Value);
    }


    [HttpGet("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetConsultationByIdQuery(id),
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return NotFound();

        var consultation = result.Value;

        var model = new UpdateConsultationCommand(
            consultation.Id,
            consultation.FullName,
            consultation.PhoneNumber,
            consultation.Message,
            consultation.Status);

        return View(model);
    }


    [HttpPost("Edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateConsultationCommand model,
        CancellationToken cancellationToken)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        var result = await mediator.Send(
            model,
            cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] =
                "Consultation updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(
            string.Empty,
            result.Message ?? "Could not update consultation.");

        return View(model);
    }


    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new DeleteConsultationCommand(id),
            cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] =
                "Consultation deleted successfully.";
        }
        else
        {
            TempData["ErrorMessage"] =
                result.Message ?? "Could not delete consultation.";
        }

        return RedirectToAction(nameof(Index));
    }
}

