using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WrocRide.Entities;
using WrocRide.Models;
using WrocRide.Services;

namespace WrocRide.Controllers
{
    [Route("api/ride")]
    [ApiController]
    [Authorize]
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public ActionResult CreateRide([FromBody] CreateRideDto dto)
        {
            int id = _rideService.CreateRide(dto);

            return Created($"api/ride/{id}", null);
        }

        [HttpGet]
        public ActionResult GetAllRides()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult GetRideById([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPut]
        public ActionResult UpdateRideStatus()
        {
            return NoContent();
        }

        [HttpPut("decision")]
        public ActionResult DriverDecision()
        {
            //this endpoint will use updaterdie method in service and inheritance after updateridedto
            return NoContent();
        }
    }
}
