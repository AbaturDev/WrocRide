
namespace WrocRide.API.Controllers
{
    [Authorize(Policy = "IsActivePolicy")]
    [Route("api/ride")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult> CreateRide([FromBody] CreateRideDto dto)
        {
            int id = await _rideService.CreateRide(dto);

            return Created($"api/ride/{id}", null);
        }

        [HttpPost("reservation")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult> CreateRideReservation([FromBody] CreateRideReservationDto dto)
        {
            int id = await _rideService.CreateRideReservation(dto);

            return Created($"api/ride/{id}", null);
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<RideDto>>> GetAllRides([FromQuery] RideQuery query)
        {
            var result = await _rideService.GetAll(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RideDeatailsDto>> GetRideById([FromRoute] int id)
        {
            var result = await _rideService.GetById(id);

            return Ok(result);
        }

        [HttpPut("{id}/ride-status")]
        public async Task<ActionResult> UpdateRideStatus([FromRoute] int id, [FromBody] UpdateRideStatusDto dto)
        {
            await _rideService.UpdateRideStatus(id, dto);

            return Ok();
        }

        [HttpPut("{id}/driver-decision")]
        public async Task<ActionResult> DriverDecision([FromRoute] int id, [FromBody] UpdateRideStatusDto dto)
        {
            await _rideService.DriverDecision(id, dto);

            return Ok();
        }

        [HttpPut("{id}/cancel-ride")]
        public async Task<ActionResult> CancelRide([FromRoute] int id)
        {
            await _rideService.CancelRide(id);

            return Ok();
        }

        [HttpPut("{id}/end-ride")]
        public async Task<ActionResult> EndRide([FromRoute] int id)
        {
            await _rideService.EndRide(id);

            return Ok();
        }
    }
}