using Clinic.Api.Common;
using Clinic.Api.Common.Validation;
using Clinic.Api.RequestModels.ClinicSettingRequestModels;
using Clinic.Application.Common;
using Clinic.Application.Features.ClinicSetting.Commands.Models;
using Clinic.Application.Features.ClinicSetting.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
    [Authorize(Roles = ApplicationRoles.Admin)]
    [ApiController]
    public class ClinicSettingsController : ApplicationControllerBase
    {
        public ClinicSettingsController(IMediator mediator) : base(mediator) { }

        [HttpGet(Router.ClinicSettingsRouting.Get)]
        public async Task<IActionResult> GetClinicSettingsAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetClinicSettingsQueryModel(), cancellationToken);
            return Ok(result);
        }

        [HttpPut(Router.ClinicSettingsRouting.Edit)]
        public async Task<IActionResult> Update(
            [FromForm] UpdateClinicSettingRequest request,
            CancellationToken cancellationToken)
        {
            IReadOnlyCollection<ClinicSettingImageUpload>? imageUploads = null;

            if (request.Images is not null && request.Images.Count > 0)
            {
                foreach (var image in request.Images)
                {
                    var error = await ImageFileValidator.ValidateAsync(image);
                    if (error is not null)
                        return BadRequest(new { Succeeded = false, Message = error });
                }

                imageUploads = request.Images
                    .Select(image => new ClinicSettingImageUpload(image.OpenReadStream(), image.FileName))
                    .ToList();
            }

            try
            {
                var command = new UpdateClinicSettingModel(
                    request.ClinicName,
                    request.DoctorName,
                    request.Specialization,
                    request.Description,
                    request.WhatsAppNumber,
                    request.Email,
                    request.Address,
                    request.FacebookUrl,
                    request.InstagramUrl,
                    imageUploads);

                var result = await _mediator.Send(command, cancellationToken);
                return Ok(result);
            }
            finally
            {
                if (imageUploads is not null)
                    foreach (var upload in imageUploads)
                        await upload.FileStream.DisposeAsync();
            }
        }
    }
}
