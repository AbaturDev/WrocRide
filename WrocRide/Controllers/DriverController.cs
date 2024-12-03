using Microsoft.AspNetCore.Mvc;
using WrocRide.Entities;
using WrocRide.Models;
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
        public ActionResult<IEnumerable<DriverDto>> Get()
        {
            var result = _driverService.GetAllAvailableDrivers();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<DriverDto> GetById([FromRoute] int id)
        {
            var result = _driverService.GetById(id);

            return Ok(result);
        }

    }
}
