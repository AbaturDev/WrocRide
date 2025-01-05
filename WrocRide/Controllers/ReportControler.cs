using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WrocRide.Models;
using WrocRide.Services;

namespace WrocRide.Controllers
{
    [Route("api/reports")]
    [ApiController]
    [Authorize(Roles = "Client, Driver")]
    public class ReportControler : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportControler(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("create")]
        public ActionResult CreateReport([FromBody] CreateReportDto dto)
        {
            _reportService.reportUser(dto);

            return Ok();
        }
    }
}
