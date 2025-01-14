namespace WrocRide.API.Controllers
{
    [Authorize(Policy = "IsActivePolicy")]
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
        public async Task<ActionResult<CarDto>> GetById([FromRoute]int driverId, [FromRoute]int carId)
        {
            var result = await _carService.GetById(driverId, carId);

            return Ok(result);
        }

        [HttpPut("{carId}")]
        public async Task<ActionResult> UpdateCar([FromRoute]int driverId, [FromRoute]int carId, [FromBody]UpdateCarDto dto)
        {
            await _carService.UpdateCar(driverId, carId, dto);

            return Ok();
        }
    }
}
