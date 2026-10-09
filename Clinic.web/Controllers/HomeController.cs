using Clinic.Application.Features.Articles.Queries.Models;
using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Features.ClinicSetting.Queries.Models;
using Clinic.Application.Features.Treatments.Queries.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Domain.Entities.artical;
using Clinic.web.Models;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Clinic.web.Controllers;

public class HomeController(IMediator mediator) : Controller
{
    private const int HomePageItemCount = 9;

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Sequential on purpose: all handlers share the same scoped DbContext,
        // which does not allow concurrent operations (no Task.WhenAll here).
        var settingsResult = await mediator.Send(
            new GetClinicSettingsQueryModel(),
            cancellationToken);

        var treatmentsResult = await mediator.Send(
            new GetAllTreatmentsQuery
            {
                PageIndex = 1,
                PageSize = HomePageItemCount
            },
            cancellationToken);

        var articlesResult = await mediator.Send(
            new GetAllArticlesQuery
            {
                PageIndex = 1,
                PageSize = HomePageItemCount,
                Status = ArticleStatus.Published
            },
            cancellationToken);

        var clinicSettings = settingsResult.IsSuccess ? settingsResult.Value : null;

        // The handler already returns the images ordered by DisplayOrder (primary first).
        var heroImages = clinicSettings?.Images ?? [];

        IReadOnlyList<AllTreatmentsDto> treatments = [];
        if (treatmentsResult.IsSuccess && treatmentsResult.Value is not null)
            treatments = treatmentsResult.Value.Items;

        IReadOnlyList<AllArticlesDto> articles = [];
        if (articlesResult.IsSuccess && articlesResult.Value is not null)
            articles = articlesResult.Value.Items;

        return View(new HomeViewModel
        {
            ClinicSettings = clinicSettings,
            HeroImages = heroImages,
            Treatments = treatments,
            Articles = articles
        });
    }

    public IActionResult SetLanguage(string culture, string? returnUrl)
    {
        if (culture != "en-US" && culture != "ar-EG")
            culture = "en-US";

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)));

        return LocalRedirect(returnUrl ?? "/");
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}