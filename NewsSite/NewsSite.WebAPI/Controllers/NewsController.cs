using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PageList<NewsResponse>), 200)]
        public async Task<ActionResult<PageList<NewsResponse>>> GetNews([FromQuery] PageSettings? pageSettings)
        {
            var pageList = await _newsService.GetNewsAsync(pageSettings);

            return Ok(pageList);
        }

        [HttpGet("by-rubric")]
        [ProducesResponseType(typeof(PageList<NewsResponse>), 200)]
        public async Task<ActionResult<PageList<NewsResponse>>> GetNewsByRubricAsync(
            [FromQuery] Guid rubricId, 
            [FromQuery] PageSettings? pageSettings)
        {
            var pageList = await _newsService.GetNewsByRubricAsync(rubricId, pageSettings);

            return Ok(pageList);
        }

        [HttpGet("by-tags")]
        [ProducesResponseType(typeof(PageList<NewsResponse>), 200)]
        public async Task<ActionResult<PageList<NewsResponse>>> GetNewsByTagsAsync(
            [FromQuery] List<Guid> tagsIds,
            [FromQuery] PageSettings? pageSettings)
        {
            var pageList = await _newsService.GetNewsByTagsAsync(tagsIds, pageSettings);

            return Ok(pageList);
        }

        [HttpGet("by-author")]
        [ProducesResponseType(typeof(PageList<NewsResponse>), 200)]
        public async Task<ActionResult<PageList<NewsResponse>>> GetNewsByAuthorAsync(
            [FromQuery] Guid authorId,
            [FromQuery] PageSettings? pageSettings)
        {
            var pageList = await _newsService.GetNewsByAuthorAsync(authorId, pageSettings);

            return Ok(pageList);
        }

        [HttpGet("by-date")]
        [ProducesResponseType(typeof(PageList<NewsResponse>), 200)]
        public async Task<ActionResult<PageList<NewsResponse>>> GetNewsByPeriodOfTimeAsync(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] PageSettings? pageSettings)
        {
            var pageList = await _newsService.GetNewsByPeriodOfTimeAsync(startDate, endDate, pageSettings);

            return Ok(pageList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NewsResponse), 200)]
        public async Task<ActionResult<PageList<NewsResponse>>> GetNewsById([FromRoute] Guid id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);

            return Ok(news);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NewsResponse), 201)]
        public async Task<ActionResult<NewsResponse>> CreateNewNewsAsync([FromBody] NewNewsRequest newNewsRequest)
        {
            var newNews = await _newsService.CreateNewNewsAsync(newNewsRequest);

            return Created(nameof(GetNewsById), newNews);
        }

        [HttpPut]
        [ProducesResponseType(typeof(NewsResponse), 200)]
        public async Task<ActionResult<NewsResponse>> UpdateNewsAsync([FromBody] UpdateNewsRequest updateNewsRequest)
        {
            var updateNews = await _newsService.UpdateNewsAsync(updateNewsRequest);

            return Ok(updateNews);
        }

        [HttpDelete("{newsId}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteNews([FromRoute] Guid newsId)
        {
            await _newsService.DeleteNewsAsync(newsId);

            return NoContent();
        }
    }
}
