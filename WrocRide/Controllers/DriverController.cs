using Microsoft.AspNetCore.Mvc;
using WrocRide.Entities;
using WrocRide.Models;
using WrocRide.Models.Enums;
using WrocRide.Services;

namespace WrocRide.Controllers
{
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

        [HttpPut("{id}/pricing")]
        public ActionResult UpdatePricing([FromRoute] int id, [FromBody] UpdateDriverPricingDto dto)
        {
            _driverService.UpdatePricing(id, dto);

            return Ok();
        }

        [HttpPut("{id}/status")]
        public ActionResult UpdateStatus([FromRoute] int id, [FromBody] UpdateDriverStatusDto dto)
        {
            _driverService.UpdateStatus(id, dto);

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
