using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsService _authorsService;

        public AuthorsController(IAuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PageList<AuthorResponse>), 200)]
        public async Task<ActionResult<PageList<AuthorResponse>>> GetAuthors([FromQuery] PageSettings? pageSettings)
        {
            var pageList = await _authorsService.GetAuthorsAsync(pageSettings);

            return Ok(pageList);
        }

        [HttpGet("{authorId}")]
        [ProducesResponseType(typeof(AuthorResponse), 200)]
        public async Task<ActionResult<PageList<AuthorResponse>>> GetAuthorById([FromRoute] Guid authorId)
        {
            var author = await _authorsService.GetAuthorByIdAsync(authorId);

            return Ok(author);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuthorResponse), 200)]
        public async Task<ActionResult<AuthorResponse>> UpdateAuthor([FromBody] UpdatedAuthorRequest updatedAuthor)
        {
            var author = await _authorsService.UpdateAuthorAsync(updatedAuthor);

            return Ok(author);
        }

        [HttpDelete("{authorId}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAuthor([FromRoute] Guid authorId)
        {
            await _authorsService.DeleteAuthorAsync(authorId);

            return NoContent();
        }
    }
}
