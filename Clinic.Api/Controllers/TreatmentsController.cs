
using Clinic.Api.Common;
using Clinic.Api.Common.Validation;
using Clinic.Api.RequestModels.TreatmentRequestsModels;
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
        public TreatmentsController(IMediator mediator)
            : base(mediator)
        {
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpGet(Router.TreatmentRouting.GetByID)]
        public async Task<IActionResult> GetTreatmentByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetTreatmentByIdQuery(id),
                cancellationToken);

            return Ok(result);
        }

        [HttpGet(Router.TreatmentRouting.GetBySlug)]
        public async Task<IActionResult> GetTreatmentBySlugAsync(
            string slug,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetTreatmentBySlugQuery(slug),
                cancellationToken);

            return Ok(result);
        }

        [HttpGet(Router.TreatmentRouting.List)]
        public async Task<IActionResult> GetAllTreatmentsAsync(
            [FromQuery] GetAllTreatmentsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                query,
                cancellationToken);

            return Ok(result);
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPost(Router.TreatmentRouting.Create)]
        public async Task<IActionResult> AddTreatment(
            [FromForm] AddTreatmentRequest request,
            CancellationToken cancellationToken)
        {
            foreach (var image in request.Images)
            {
                var validationError =
                    await ImageFileValidator.ValidateAsync(image);

                if (validationError is not null)
                {
                    return BadRequest(new
                    {
                        Succeeded = false,
                        Message = validationError
                    });
                }
            }

            var imageUploads = new List<TreatmentImageUpload>();

            try
            {
                foreach (var image in request.Images)
                {
                    imageUploads.Add(
                        new TreatmentImageUpload(
                            image.OpenReadStream(),
                            image.FileName));
                }

                var command = new AddTreatmentCommand(
                    request.Name,
                    request.Description,
                    imageUploads);

                var result = await _mediator.Send(
                    command,
                    cancellationToken);

                return Ok(result);
            }
            finally
            {
                foreach (var imageUpload in imageUploads)
                {
                    await imageUpload.FileStream.DisposeAsync();
                }
            }
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPut(Router.TreatmentRouting.Edit)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromForm] UpdateTreatmentRequest request,
            CancellationToken cancellationToken)
        {
            IReadOnlyCollection<TreatmentImageUpload>? imageUploads = null;

            if (request.Images is not null &&
                request.Images.Count > 0)
            {
                foreach (var image in request.Images)
                {
                    var validationError =
                        await ImageFileValidator.ValidateAsync(image);

                    if (validationError is not null)
                    {
                        return BadRequest(new
                        {
                            Succeeded = false,
                            Message = validationError
                        });
                    }
                }

                imageUploads = request.Images
                    .Select(image =>
                        new TreatmentImageUpload(
                            image.OpenReadStream(),
                            image.FileName))
                    .ToList();
            }

            try
            {
                var command = new UpdateTreatmentCommand(
                    id,
                    request.Name,
                    request.Description,
                    imageUploads);

                var result = await _mediator.Send(
                    command,
                    cancellationToken);

                return Ok(result);
            }
            finally
            {
                if (imageUploads is not null)
                {
                    foreach (var imageUpload in imageUploads)
                    {
                        await imageUpload.FileStream.DisposeAsync();
                    }
                }
            }
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpDelete(Router.TreatmentRouting.Delete)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new DeleteTreatmentCommand(id),
                cancellationToken);

            return Ok(result);
        }
    }
}

