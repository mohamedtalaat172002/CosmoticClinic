using Clinic.Api.Common;
using Clinic.Api.Common.Validation;
using Clinic.Api.RequestModels.ArticleRequestsModels;
using Clinic.Application.Common;
using Clinic.Application.Features.Articles.Commands.Models;
using Clinic.Application.Features.Articles.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
    [ApiController]
    public class ArticlesController : ApplicationControllerBase
    {
        public ArticlesController(IMediator mediator) : base(mediator) { }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpGet(Router.ArticleRouting.GetByID)]
        public async Task<IActionResult> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetArticleByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpGet(Router.ArticleRouting.GetBySlug)]
        public async Task<IActionResult> GetArticleBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetArticleBySlugQuery(slug), cancellationToken);
            return Ok(result);
        }

        [HttpGet(Router.ArticleRouting.List)]
        public async Task<IActionResult> GetAllArticlesAsync(
            [FromQuery] GetAllArticlesQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPost(Router.ArticleRouting.Create)]
        public async Task<IActionResult> AddArticle(
            [FromForm] AddArticleRequest request,
            CancellationToken cancellationToken)
        {
            foreach (var image in request.Images)
            {
                var error = await ImageFileValidator.ValidateAsync(image);
                if (error is not null)
                    return BadRequest(new { Succeeded = false, Message = error });
            }

            var imageUploads = new List<ArticleImageUpload>();
            try
            {
                foreach (var image in request.Images)
                    imageUploads.Add(new ArticleImageUpload(image.OpenReadStream(), image.FileName));

                var command = new AddArticleCommand(
                    request.Title,
                    request.Summary,
                    request.Content,
                    request.Status,
                    request.TreatmentId,
                    imageUploads);

                var result = await _mediator.Send(command, cancellationToken);
                return Ok(result);
            }
            finally
            {
                foreach (var upload in imageUploads)
                    await upload.FileStream.DisposeAsync();
            }
        }

        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPut(Router.ArticleRouting.Edit)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromForm] UpdateArticleRequest request,
            CancellationToken cancellationToken)
        {
            IReadOnlyCollection<ArticleImageUpload>? imageUploads = null;

            if (request.Images is not null && request.Images.Count > 0)
            {
                foreach (var image in request.Images)
                {
                    var error = await ImageFileValidator.ValidateAsync(image);
                    if (error is not null)
                        return BadRequest(new { Succeeded = false, Message = error });
                }

                imageUploads = request.Images
                    .Select(image => new ArticleImageUpload(image.OpenReadStream(), image.FileName))
                    .ToList();
            }

            try
            {
                var command = new UpdateArticleCommand(
                    id,
                    request.Title,
                    request.Summary,
                    request.Content,
                    request.Status,
                    request.TreatmentId,
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

        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpDelete(Router.ArticleRouting.Delete)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteArticleCommand(id), cancellationToken);
            return Ok(result);
        }
    }
}
