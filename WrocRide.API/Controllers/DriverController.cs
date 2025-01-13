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
        public ActionResult<IEnumerable<DriverDto>> Get([FromQuery] DriverQuery query)
        {
            var result = _driverService.GetAll(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<DriverDto> GetById([FromRoute] int id)
        {
            var result = _driverService.GetById(id);

            return Ok(result);
        }

        [HttpPut("pricing")]
        public ActionResult UpdatePricing([FromBody] UpdateDriverPricingDto dto)
        {
            _driverService.UpdatePricing(dto);

            return Ok();
        }

        [HttpPut("status")]
        public ActionResult UpdateStatus([FromBody] UpdateDriverStatusDto dto)
        {
            _driverService.UpdateStatus(dto);

            return Ok();
        }

        [HttpGet("{id}/ratings")]
        public ActionResult<IEnumerable<RatingDto>> GetRatings([FromRoute] int id, [FromQuery] DriverRatingsQuery query)
        {
            var result = _driverService.GetRatings(id, query);

            return Ok(result);
        }

    }
}
