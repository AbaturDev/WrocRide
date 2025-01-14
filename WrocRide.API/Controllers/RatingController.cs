namespace WrocRide.API.Controllers
{
    [Authorize(Roles = "Client")]
    [Authorize(Policy = "IsActivePolicy")]
    [Route("api/ride/{rideId}/rating")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromRoute] int rideId, [FromBody] CreateRatingDto dto)
        {
            int id = await _ratingService.CreateRating(rideId, dto);

            return Created($"api/ride/{rideId}/rating/{id}", null);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<RatingDto>> Get([FromRoute] int rideId)
        {
            var result = await _ratingService.Get(rideId);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromRoute] int rideId)
        {
            await _ratingService.Delete(rideId);

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromRoute] int rideId, [FromBody] CreateRatingDto dto)
        {
            await _ratingService.Update(rideId, dto);

            return Ok();
        }
    }
}
