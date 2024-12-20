using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WrocRide.Helpers;
using WrocRide.Models;
using WrocRide.Services;

namespace WrocRide.Controllers
{
    [Route("api/ride/{rideId}/rating")]
    [ApiController]
    [Authorize(Roles = "Client")]
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
