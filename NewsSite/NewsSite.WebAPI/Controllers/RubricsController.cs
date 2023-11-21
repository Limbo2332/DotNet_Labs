using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RubricsController : ControllerBase
    {
        private readonly IRubricsService _rubricsService;

        public RubricsController(IRubricsService rubricsService)
        {
            _rubricsService = rubricsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PageList<RubricResponse>), 200)]
        public async Task<ActionResult<PageList<RubricResponse>>> GetRubrics([FromQuery] PageSettings? pageSettings)
        {
            var pageList = await _rubricsService.GetAllRubricsAsync(pageSettings);

            return Ok(pageList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RubricResponse), 200)]
        public async Task<ActionResult<PageList<RubricResponse>>> GetRubricById([FromRoute] Guid id)
        {
            var rubric = await _rubricsService.GetRubricByIdAsync(id);

            return Ok(rubric);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RubricResponse), 201)]
        public async Task<ActionResult<RubricResponse>> CreateNewRubricAsync([FromBody] NewRubricRequest newRubricRequest)
        {
            var newRubric = await _rubricsService.CreateNewRubricAsync(newRubricRequest);

            return Created(nameof(GetRubricById), newRubric);
        }

        [HttpPut]
        [ProducesResponseType(typeof(RubricResponse), 200)]
        public async Task<ActionResult<RubricResponse>> UpdateRubricAsync([FromBody] UpdateRubricRequest updateRubricRequest)
        {
            var updateRubric = await _rubricsService.UpdateRubricAsync(updateRubricRequest);

            return Ok(updateRubric);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteRubric([FromRoute] Guid id)
        {
            await _rubricsService.DeleteRubricAsync(id);

            return NoContent();
        }
    }
}
