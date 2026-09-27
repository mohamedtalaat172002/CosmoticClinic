using Clinic.Api.Common;
using Clinic.Application.Common;
using Clinic.Application.Features.Treatments.Commands.Models;
using Clinic.Application.Features.Treatments.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
    [ApiController]
    public class TreatmentsController : ApplicationControllerBase
    {
        public TreatmentsController(IMediator mediator) : base(mediator)
        {
        }



        [HttpGet(Router.TreatmentRouting.GetByID)]
        public async Task<IActionResult> GetTreatmentByIdAsync(
      Guid id,
      [FromQuery] GetTreatmentByIdQuery query,
      CancellationToken cancellationToken)
        {
            query.Id = id;
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        [HttpGet(Router.TreatmentRouting.GetBySlug)]
        public async Task<IActionResult> GetTreatmentBySlugAsync(
 string slug,
 [FromQuery] GetTreatmentBySlugQuery query,
 CancellationToken cancellationToken)
        {
            query.Slug = slug;

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet(Router.TreatmentRouting.List)]
        public async Task<IActionResult> GetAllTreatmentsAsync([FromQuery] GetAllTreatmentsQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        //[Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPut(Router.TreatmentRouting.Edit)]
        public virtual async Task<IActionResult> Update(Guid id, [FromBody] UpdateTreatmentCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command with { Id = id }, cancellationToken);
            return Ok(result);
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpDelete(Router.TreatmentRouting.Delete)]
        public virtual async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteTreatmentCommand(id), cancellationToken);
            return Ok(result);
        }
        //[Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPost(Router.TreatmentRouting.Create)]
        public async Task<IActionResult> AddTreatment([FromBody] AddTreatmentCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);

        }

    }
}
