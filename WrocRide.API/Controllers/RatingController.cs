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
        public ActionResult Create([FromRoute] int rideId, [FromBody] CreateRatingDto dto)
        {
            int id = _ratingService.CreateRating(rideId, dto);

            return Created($"api/ride/{rideId}/rating/{id}", null);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<RatingDto> Get([FromRoute] int rideId)
        {
            var result = _ratingService.Get(rideId);

            return Ok(result);
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int rideId)
        {
            _ratingService.Delete(rideId);

            return NoContent();
        }

        [HttpPut]
        public ActionResult Update([FromRoute] int rideId, [FromBody] CreateRatingDto dto)
        {
            _ratingService.Update(rideId, dto);

            return Ok();
        }
    }
}
