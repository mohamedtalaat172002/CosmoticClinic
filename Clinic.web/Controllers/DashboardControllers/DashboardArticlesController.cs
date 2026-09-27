using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Features.Articles.Commands.Models;
using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Domain.Entities.artical;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

[Authorize(Roles = "Admin")]
[Route("Dashboard/Articles")]
public sealed class DashboardArticlesController(IMediator mediator) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(
        string? search = null,
        int pageIndex = 1,
        int pageSize = 9,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetAllArticlesQuery
            {
                Search = search,
                PageIndex = pageIndex,
                PageSize = pageSize
            },
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
        {
            return View(
                new PagedResult<AllArticlesDto>(
                    [],
                    0,
                    pageIndex,
                    pageSize));
        }

        return View(result.Value);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(
            new AddArticleCommand(
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                ArticleStatus.Draft));
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        AddArticleCommand model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await mediator.Send(model, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Article added successfully.";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(
            string.Empty,
            result.Message ?? "Could not add article.");

        return View(model);
    }

    [HttpGet("Edit/{slug}")]
    public async Task<IActionResult> Edit(
        string slug,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetArticleBySlugQuery(slug),
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return NotFound();

        var article = result.Value;

        var model = new UpdateArticleCommand(
            article.Id,
            article.Title,
            article.Summary,
            article.Content,
            article.FeaturedImageUrl,
            article.Status);

        return View(model);
    }

    [HttpPost("Edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdateArticleCommand model,
        CancellationToken cancellationToken)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        var result = await mediator.Send(model, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Article updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(
            string.Empty,
            result.Message ?? "Could not update article.");

        return View(model);
    }

    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new DeleteArticleCommand(id),
            cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Article deleted successfully.";
        }
        else
        {
            TempData["ErrorMessage"] =
                result.Message ?? "Could not delete article.";
        }

        return RedirectToAction(nameof(Index));
    }
}

