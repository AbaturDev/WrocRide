using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.API.Controllers
{
    [Authorize(Policy = "IsActivePolicy")]
    [Route("api/driver")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverDto>>> Get([FromQuery] DriverQuery query)
        {
            var result = await _driverService.GetAll(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriverDto>> GetById([FromRoute] int id)
        {
            var result = await _driverService.GetById(id);

            return Ok(result);
        }

        [HttpPut("pricing")]
        public async Task<ActionResult> UpdatePricing([FromBody] UpdateDriverPricingDto dto)
        {
            await _driverService.UpdatePricing(dto);

            return Ok();
        }

        [HttpPut("status")]
        public async Task<ActionResult> UpdateStatus([FromBody] UpdateDriverStatusDto dto)
        {
            await _driverService.UpdateStatus(dto);

            return Ok();
        }

        [HttpGet("{id}/ratings")]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatings([FromRoute] int id, [FromQuery] PageQuery query)
        {
            var result = await _driverService.GetRatings(id, query);

            return Ok(result);
        }

    }
}
