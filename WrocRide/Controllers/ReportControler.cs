using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WrocRide.Models;
using WrocRide.Services;

namespace WrocRide.Controllers
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
        public ActionResult CreateReport([FromRoute] int rideId, [FromBody] CreateReportDto dto)
        {
            _reportService.CreateReport(rideId, dto);

            return Ok();
        }

        [HttpGet]
        public ActionResult<ReportDto> Get([FromRoute] int rideId)
        {
            var result = _reportService.Get(rideId);

            return Ok(result);
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int rideId)
        {
            _reportService.Delete(rideId);

            return NoContent();
        }

        [HttpPut]
        public ActionResult Update([FromRoute] int rideId, [FromBody] CreateReportDto dto)
        {
            _reportService.Update(rideId, dto);

            return Ok();
        }
    }
}
