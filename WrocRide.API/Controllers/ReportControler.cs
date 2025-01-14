
namespace WrocRide.API.Controllers
{
    [Route("api/ride/{rideId}/report")]
    [ApiController]
    [Authorize(Roles = "Client, Driver")]
    [Authorize(Policy = "IsActivePolicy")]
    public class ReportControler : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportControler(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateReport([FromRoute] int rideId, [FromBody] CreateReportDto dto)
        {
            await _reportService.CreateReport(rideId, dto);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<ReportDto>> Get([FromRoute] int rideId)
        {
            var result = await _reportService.Get(rideId);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromRoute] int rideId)
        {
            await _reportService.Delete(rideId);

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromRoute] int rideId, [FromBody] CreateReportDto dto)
        {
            await _reportService.Update(rideId, dto);

            return Ok();
        }
    }
}
