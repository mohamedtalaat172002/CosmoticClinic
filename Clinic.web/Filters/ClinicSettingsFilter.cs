using Clinic.Application.Features.ClinicSetting.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clinic.web.Filters;

/// <summary>
/// Runs on every MVC action and populates ViewData["ClinicSettings"]
/// so the shared layout always has live clinic data without touching controllers.
/// </summary>
public sealed class ClinicSettingsFilter(IMediator mediator) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var result = await mediator.Send(new GetClinicSettingsQueryModel());

        if (context.Controller is Controller controller)
            controller.ViewData["ClinicSettings"] = result.Value;

        await next();
    }
}
