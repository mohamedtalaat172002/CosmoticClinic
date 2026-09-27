using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.ClinicSetting.Queries.Models;
using Clinic.Application.Features.Treatments.Queries.Models;
using Clinic.web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Clinic.web.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        const int homePageItemCount = 9;

        var treatmentsResult = await _mediator.Send(
            new GetAllTreatmentsQuery
            {
                PageSize = homePageItemCount
            },
            cancellationToken);

        var articlesResult = await _mediator.Send(
            new GetAllArticlesQuery
            {
                PageSize = homePageItemCount
            },
            cancellationToken);

        var clinicSettingsResult = await _mediator.Send(
            new GetClinicSettingsQueryModel(),
            cancellationToken);

        return View(new HomeViewModel
        {
            ClinicSettings = clinicSettingsResult.Value,
            Treatments = treatmentsResult.Value?.Items ?? [],
            Articles = articlesResult.Value?.Items ?? []
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id
                ?? HttpContext.TraceIdentifier
        });
    }

}
