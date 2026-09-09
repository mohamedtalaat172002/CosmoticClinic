using Clinic.Api.Common;
using Clinic.Application.Features.Articles.Commands.Models;
using Clinic.Application.Features.Articles.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ApplicationControllerBase
    {
        public ArticlesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("Get-Article-ById")]
        public async Task<IActionResult> GetArticleByIdASync([FromQuery] GetArticleByIdQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("Get-All-Article")]
        public async Task<IActionResult> GetAllArticlesAsync([FromQuery] GetAllArticlesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpPut("Update-Article")]
        public virtual async Task<IActionResult> Update([FromBody] UpdateArticleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("Delete-Article")]
        public virtual async Task<IActionResult> Delete([FromBody] DeleteArticleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }



        [HttpPost("Add-Article")]
        public async Task<IActionResult> AddArticle([FromBody] AddArticleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
