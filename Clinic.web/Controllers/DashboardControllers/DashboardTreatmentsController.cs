using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Features.Treatments.Commands.Models;
using Clinic.Application.Features.Treatments.Queries.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.web.Models;
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
            return View(new PagedResult<AllTreatmentsDto>([], 0, pageIndex, pageSize));

        return View(result.Value);
    }

    // GET /Dashboard/Treatments/Create
    [HttpGet("Create")]
    public IActionResult Create() => View(new TreatmentUpsertInputModel());

    // POST /Dashboard/Treatments/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        TreatmentUpsertInputModel model,
        CancellationToken cancellationToken)
    {
        var imageError = ValidateImages(model.Images);
        if (imageError is not null)
            ModelState.AddModelError(nameof(model.Images), imageError);

        if (!ModelState.IsValid)
            return View(model);

        var uploads = ToUploads(model.Images);
        try
        {
            var result = await mediator.Send(
                new AddTreatmentCommand(
                    model.Name,
                    model.Description,
                    uploads),
                cancellationToken);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Treatment added successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Could not add treatment.");
        }
        finally
        {
            foreach (var upload in uploads)
                await upload.FileStream.DisposeAsync();
        }

        return View(model);
    }

    // GET /Dashboard/Treatments/Edit/{id}
    // By Id (not slug): the slug changes when the name changes, and the public slug
    // query also loads a page of articles that the edit form does not need.
    [HttpGet("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetTreatmentByIdQuery(id), cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return NotFound();

        var treatment = result.Value;

        return View(new TreatmentUpsertInputModel
        {
            Id = treatment.Id,
            Name = treatment.Name,
            Description = treatment.Description,
            ExistingImages = treatment.Images
        });
    }

    // POST /Dashboard/Treatments/Edit/{id}
    // HTML forms can only POST; [HttpPut] never matched the edit form.
    [HttpPost("Edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        TreatmentUpsertInputModel model,
        CancellationToken cancellationToken)
    {
        if (id != model.Id)
            return BadRequest();

        var imageError = ValidateImages(model.Images);
        if (imageError is not null)
            ModelState.AddModelError(nameof(model.Images), imageError);

        if (!ModelState.IsValid)
        {
            model.ExistingImages = await LoadExistingImagesAsync(id, cancellationToken);
            return View(model);
        }

        var uploads = ToUploads(model.Images);
        try
        {
            // New images (if any) replace ALL existing images; null keeps the old ones.
            var result = await mediator.Send(
                new UpdateTreatmentCommand(
                    model.Id,
                    model.Name,
                    model.Description,
                    uploads.Count > 0 ? uploads : null),
                cancellationToken);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Treatment updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Could not update treatment.");
        }
        finally
        {
            foreach (var upload in uploads)
                await upload.FileStream.DisposeAsync();
        }

        model.ExistingImages = await LoadExistingImagesAsync(id, cancellationToken);
        return View(model);
    }

    // POST /Dashboard/Treatments/Delete/{id}
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteTreatmentCommand(id), cancellationToken);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Treatment deleted successfully.";
        else
            TempData["ErrorMessage"] = result.Message ?? "Could not delete treatment.";

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<TreatmentImageDto>> LoadExistingImagesAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetTreatmentByIdQuery(id), cancellationToken);

        return result.IsSuccess && result.Value is not null
            ? result.Value.Images
            : new List<TreatmentImageDto>();
    }

    private static List<TreatmentImageUpload> ToUploads(IEnumerable<IFormFile>? files) =>
        files?
            .Where(f => f.Length > 0)
            .Select(f => new TreatmentImageUpload(f.OpenReadStream(), f.FileName))
            .ToList()
        ?? [];

    private static string? ValidateImages(IEnumerable<IFormFile>? files)
    {
        if (files is null)
            return null;

        foreach (var file in files.Where(f => f.Length > 0))
        {
            if (file.Length > 5 * 1024 * 1024)
                return "The image size must not exceed 5 MB.";

            if (!file.ContentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase)
                && !file.ContentType.Equals("image/png", StringComparison.OrdinalIgnoreCase)
                && !file.ContentType.Equals("image/webp", StringComparison.OrdinalIgnoreCase))
            {
                return "Only JPG, JPEG, PNG, and WEBP images are allowed.";
            }
        }

        return null;
    }
}