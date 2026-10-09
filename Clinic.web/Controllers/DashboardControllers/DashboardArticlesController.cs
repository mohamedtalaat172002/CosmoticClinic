using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Features.Articles.Commands.Models;
using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Domain.Entities.artical;
using Clinic.web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

[Authorize(Roles = "Admin")]
[Route("Dashboard/Articles")]
public sealed class DashboardArticlesController(IMediator mediator) : Controller
{
    // GET /Dashboard/Articles
    [HttpGet("")]
    public async Task<IActionResult> Index(
        string? search = null,
        ArticleStatus? status = null,
        int pageIndex = 1,
        int pageSize = 9,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetAllArticlesQuery
            {
                Search = search,
                Status = status,
                PageIndex = pageIndex,
                PageSize = pageSize
            },
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return View(new PagedResult<AllArticlesDto>([], 0, pageIndex, pageSize));

        return View(result.Value);
    }

    // GET /Dashboard/Articles/Create
    [HttpGet("Create")]
    public IActionResult Create() => View(new ArticleUpsertInputModel());

    // POST /Dashboard/Articles/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ArticleUpsertInputModel model,
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
                new AddArticleCommand(
                    model.Title,
                    model.Summary,
                    model.Content,
                    model.Status,
                    model.TreatmentId,
                    uploads),
                cancellationToken);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Article added successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Could not add article.");
        }
        finally
        {
            foreach (var upload in uploads)
                await upload.FileStream.DisposeAsync();
        }

        return View(model);
    }

    [HttpGet("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(
      Guid id,
      CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetArticleByIdQuery(id), cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return NotFound();

        var article = result.Value;

        return View(new ArticleUpsertInputModel
        {
            Id = article.Id,
            Title = article.Title,
            Summary = article.Summary,
            Content = article.Content,
            Status = article.Status,
            TreatmentId = article.TreatmentId,
            ExistingImages = article.Images
        });
    }


    [HttpPost("Edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        ArticleUpsertInputModel model,
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
                new UpdateArticleCommand(
                    model.Id,
                    model.Title,
                    model.Summary,
                    model.Content,
                    model.Status,
                    model.TreatmentId,
                    uploads.Count > 0 ? uploads : null),
                cancellationToken);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Article updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Could not update article.");
        }
        finally
        {
            foreach (var upload in uploads)
                await upload.FileStream.DisposeAsync();
        }

        model.ExistingImages = await LoadExistingImagesAsync(id, cancellationToken);
        return View(model);
    }

    // POST /Dashboard/Articles/Delete/{id}
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteArticleCommand(id), cancellationToken);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Article deleted successfully.";
        else
            TempData["ErrorMessage"] = result.Message ?? "Could not delete article.";

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<ArticleImageDto>> LoadExistingImagesAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetArticleByIdQuery(id), cancellationToken);

        return result.IsSuccess && result.Value is not null
            ? result.Value.Images
            : new List<ArticleImageDto>();
    }

    private static List<ArticleImageUpload> ToUploads(IEnumerable<IFormFile>? files) =>
        files?
            .Where(f => f.Length > 0)
            .Select(f => new ArticleImageUpload(f.OpenReadStream(), f.FileName))
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