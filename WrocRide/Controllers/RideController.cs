using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WrocRide.Entities;
using WrocRide.Services;

namespace WrocRide.Controllers
{
    [Route("api/ride")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }
    }
}
