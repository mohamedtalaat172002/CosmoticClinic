using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Domain.Entities.artical;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

public sealed class ArticlesController(IMediator mediator) : Controller
{
    [HttpGet]
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
                PageSize = pageSize,
                Status = ArticleStatus.Published
            },
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return View(new PagedResult<AllArticlesDto>([], 0, pageIndex, pageSize));

        return View(result.Value);
    }

    [HttpGet("articles/{slug}")]
    public async Task<IActionResult> Details(
        string slug,
        CancellationToken cancellationToken = default)
    {

        var result = await mediator.Send(
            new GetArticleBySlugQuery(slug),
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return NotFound();

        return View(result.Value);
    }
}