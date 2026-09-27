using Clinic.Application.Abstractions.Queries;
using Clinic.Application.Features.Treatments.Queries.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

public sealed class TreatmentsController(IMediator mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(
        string? search = null,
        int pageIndex = 1,
        int pageSize = 6,
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

    [HttpGet("treatments/{slug}")]
    public async Task<IActionResult> Details(
        string slug,
        string? search = null,
        int pageIndex = 1,
        int pageSize = 6,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetTreatmentBySlugQuery
            {
                Slug = slug,
                Search = search,
                PageIndex = pageIndex,
                PageSize = pageSize
            },
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
            return NotFound();

        return View(result.Value);
    }
}
