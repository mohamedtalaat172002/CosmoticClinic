
using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Features.Treatments.Commands.Models;
using Clinic.Application.Features.Treatments.Queries.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

[Authorize(Roles = "Admin")]
[Route("Dashboard/Treatments")]
public sealed class DashboardTreatmentsController(IMediator mediator) : Controller
{
    // GET /Dashboard/Treatments
    [HttpGet("")]
    public async Task<IActionResult> Index(
        string? search = null,
        int pageIndex = 1,
        int pageSize = 9,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetAllTreatmentsQuery
            {
                Search = search,
                PageIndex = pageIndex,
                PageSize = pageSize
            },
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
        {
            return View(
                new PagedResult<AllTreatmentsDto>(
                    [],
                    0,
                    pageIndex,
                    pageSize));
        }

        return View(result.Value);
    }


    // GET /Dashboard/Treatments/Create
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new AddTreatmentCommand(
            string.Empty,
            string.Empty,
            null));
    }


    // POST /Dashboard/Treatments/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        AddTreatmentCommand model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await mediator.Send(model, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Treatment added successfully.";

            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(
            string.Empty,
            result.Message ?? "Could not add treatment.");

        return View(model);
    }


    // GET /Dashboard/Treatments/Edit/{slug}
    [HttpGet("Edit/{slug}")]
    public async Task<IActionResult> Edit(
        string slug,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetTreatmentBySlugQuery
            {
                Slug = slug,
                PageIndex = 1,
                PageSize = 1
            },
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return NotFound();

        var treatment = result.Value;

        var model = new UpdateTreatmentCommand(
            treatment.Id,
            treatment.Name,
            treatment.Description,
            treatment.ImageUrl);

        return View(model);
    }


    // POST /Dashboard/Treatments/Edit/{id}
    [HttpPost("Edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateTreatmentCommand model,
        CancellationToken cancellationToken)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        var result = await mediator.Send(model, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Treatment updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(
            string.Empty,
            result.Message ?? "Could not update treatment.");

        return View(model);
    }


    // POST /Dashboard/Treatments/Delete/{id}
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new DeleteTreatmentCommand(id),
            cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Treatment deleted successfully.";
        }
        else
        {
            TempData["ErrorMessage"] =
                result.Message ?? "Could not delete treatment.";
        }

        return RedirectToAction(nameof(Index));
    }
}
