using Clinic.Api.Common;
using Clinic.Application.Common;
using Clinic.Application.Features.Consultations.Commands.Models;
using Clinic.Application.Features.Consultations.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{

    [ApiController]
    public class ConsultationsController : ApplicationControllerBase
    {
        public ConsultationsController(IMediator mediator) : base(mediator)
        {
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpGet(Router.ConsultationRouting.GetByID)]
        public async Task<IActionResult> GetConsultationByIdAsync(Guid id, [FromQuery] GetConsultationByIdQuery query, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetConsultationByIdQuery(id), cancellationToken);
            return Ok(result);
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpGet(Router.ConsultationRouting.List)]
        public async Task<IActionResult> GetAllConsultationsAsync([FromQuery] GetAllConsultationQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPut(Router.ConsultationRouting.Edit)]
        public virtual async Task<IActionResult> Update(Guid id, [FromBody] UpdateConsultationCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command with { Id = id }, cancellationToken);
            return Ok(result);
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpDelete(Router.ConsultationRouting.Delete)]
        public virtual async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteConsultationCommand(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost(Router.ConsultationRouting.Create)]
        public async Task<IActionResult> AddConsultation([FromBody] AddConsultationCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
