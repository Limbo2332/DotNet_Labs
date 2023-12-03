using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.UI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagsService;

        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PageList<TagResponse>), 200)]
        public async Task<ActionResult<PageList<TagResponse>>> GetTags([FromQuery] PageSettings? pageSettings)
        {
            var pageList = await _tagsService.GetTagsAsync(pageSettings);

            return Ok(pageList);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TagResponse), 200)]
        public async Task<ActionResult<PageList<TagResponse>>> GetTagById([FromRoute] Guid id)
        {
            var tag = await _tagsService.GetTagByIdAsync(id);

            return Ok(tag);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TagResponse), 201)]
        public async Task<ActionResult<TagResponse>> CreateNewTag([FromBody] NewTagRequest newTagRequest)
        {
            var newTag = await _tagsService.CreateNewTagAsync(newTagRequest);

            return Created(nameof(GetTagById), newTag);
        }

        [HttpPost("newsTag")]
        [ProducesResponseType(typeof(TagResponse), 201)]
        public async Task<ActionResult<TagResponse>> AddTagForNews([FromQuery] Guid tagId, [FromQuery] Guid newsId)
        {
            var tag = await _tagsService.AddTagForNewsIdAsync(tagId, newsId);

            return Created(nameof(GetTagById), tag);
        }

        [HttpPut]
        [ProducesResponseType(typeof(TagResponse), 200)]
        public async Task<ActionResult<TagResponse>> UpdateTag([FromBody] UpdateTagRequest updateTagRequest)
        {
            var updateTag = await _tagsService.UpdateTagAsync(updateTagRequest);

            return Ok(updateTag);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteTag([FromRoute] Guid id)
        {
            await _tagsService.DeleteTagAsync(id);

            return NoContent();
        }

        [HttpDelete("newsTag")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteTagForNews([FromQuery] Guid tagId, Guid newsId)
        {
            await _tagsService.DeleteTagForNewsIdAsync(tagId, newsId);

            return NoContent();
        }
    }
}
