using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.UI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RubricsController : ControllerBase
    {
        private readonly IRubricsService _rubricsService;

        public RubricsController(IRubricsService rubricsService)
        {
            _rubricsService = rubricsService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PageList<RubricResponse>), 200)]
        public async Task<ActionResult<PageList<RubricResponse>>> GetRubrics([FromQuery] PageSettings? pageSettings)
        {
            var pageList = await _rubricsService.GetRubricsAsync(pageSettings);

            return Ok(pageList);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(RubricResponse), 200)]
        public async Task<ActionResult<PageList<RubricResponse>>> GetRubricById([FromRoute] Guid id)
        {
            var rubric = await _rubricsService.GetRubricByIdAsync(id);

            return Ok(rubric);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RubricResponse), 201)]
        public async Task<ActionResult<RubricResponse>> CreateNewRubric([FromBody] NewRubricRequest newRubricRequest)
        {
            var newRubric = await _rubricsService.CreateNewRubricAsync(newRubricRequest);

            return Created(nameof(GetRubricById), newRubric);
        }

        [HttpPost("newsRubrics")]
        [ProducesResponseType(typeof(RubricResponse), 201)]
        public async Task<ActionResult<RubricResponse>> AddRubricForNews([FromQuery] Guid rubricId, [FromQuery] Guid newsId)
        {
            var rubric = await _rubricsService.AddRubricForNewsIdAsync(rubricId, newsId);

            return Created(nameof(GetRubricById), rubric);
        }

        [HttpPut]
        [ProducesResponseType(typeof(RubricResponse), 200)]
        public async Task<ActionResult<RubricResponse>> UpdateRubric([FromBody] UpdateRubricRequest updateRubricRequest)
        {
            var updateRubric = await _rubricsService.UpdateRubricAsync(updateRubricRequest);

            return Ok(updateRubric);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteRubric([FromRoute] Guid id)
        {
            await _rubricsService.DeleteRubricAsync(id);

            return NoContent();
        }

        [HttpDelete("newsRubrics")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteRubricForNews([FromQuery] Guid rubricId, Guid newsId)
        {
            await _rubricsService.DeleteRubricForNewsIdAsync(rubricId, newsId);

            return NoContent();
        }
    }
}
