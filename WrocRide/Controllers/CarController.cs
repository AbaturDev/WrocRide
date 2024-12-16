using Microsoft.AspNetCore.Mvc;
using WrocRide.Models;
using WrocRide.Services;

namespace WrocRide.Controllers
{
    [Route("api/driver/{driverId}/car")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("{carId}")]
        public ActionResult<CarDto> GetById([FromRoute]int driverId, [FromRoute]int carId)
        {
            var result = _carService.GetById(driverId, carId);

            return Ok(result);
        }

        [HttpPut("{carId}")]
        public ActionResult UpdateCar([FromRoute]int driverId, [FromRoute]int carId, [FromBody]UpdateCarDto dto)
        {
            _carService.UpdateCar(driverId, carId, dto);

            return Ok();
        }
    }
}
