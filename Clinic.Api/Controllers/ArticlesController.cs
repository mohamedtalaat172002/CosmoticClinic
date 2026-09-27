using Clinic.Api.Common;
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
        public ArticlesController(IMediator mediator) : base(mediator)
        {
        }

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
        //[Authorize(Roles = ApplicationRoles.Admin)]
        [HttpGet(Router.ArticleRouting.List)]
        public async Task<IActionResult> GetAllArticlesAsync([FromQuery] GetAllArticlesQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpPut(Router.ArticleRouting.Edit)]
        public virtual async Task<IActionResult> Update(Guid id, [FromBody] UpdateArticleCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command with { Id = id }, cancellationToken);
            return Ok(result);
        }
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpDelete(Router.ArticleRouting.Delete)]
        public virtual async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteArticleCommand(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost(Router.ArticleRouting.Create)]
        public async Task<IActionResult> AddArticle([FromBody] AddArticleCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}