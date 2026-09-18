using Clinic.Api.Common;
using Clinic.Application.Common;
using Clinic.Application.Features.ClinicSetting.Commands.Models;
using Clinic.Application.Features.ClinicSetting.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
    [ApiController]
    public class ClinicSettingsController : ApplicationControllerBase
    {
        public ClinicSettingsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet(Router.ClinicSettingsRouting.Get)]
        public async Task<IActionResult> GetClinicSettingsAsync(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetClinicSettingsQueryModel(),
                cancellationToken);

            return Ok(result);
        }

        [HttpPut(Router.ClinicSettingsRouting.Edit)]
        public async Task<IActionResult> Update(
            [FromBody] UpdateClinicSettingModel command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            return Ok(result);
        }
    }
}