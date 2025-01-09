using Microsoft.AspNetCore.Mvc;
using WrocRide.Services;
using WrocRide.Models;
using Microsoft.AspNetCore.Authorization;

namespace WrocRide.Controllers
{
    [Authorize(Policy = "IsActivePolicy")]
    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("documents")]
        public ActionResult<IEnumerable<DocumentDto>> GetDocuments([FromQuery] DocumentQuery query)
        {
            var result = _adminService.GetDocuments(query);

            return Ok(result);
        }

        [HttpPut("document/{id}")]
        public ActionResult UpdateDocument([FromRoute] int id, [FromBody] UpdateDocumentDto dto)
        {
            _adminService.UpdateDocument(id, dto);

            return Ok();
        }

        [HttpGet("document/{driverId}")]
        public ActionResult<DocumentDto> GetDocumentByDriverId([FromRoute] int driverId)
        {
            var result = _adminService.GetDocumentByDriverId(driverId);

            return Ok(result);
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<UserDto>> GetUsers([FromQuery] UserQuery query)
        {
            var result = _adminService.GetAll(query);

            return Ok(result);
        }

        [HttpPut("user/{userId}")]
        public ActionResult UpdateUser([FromRoute] int userId, [FromBody] UpdateUserDto dto)
        {
            _adminService.UpdateUser(userId, dto);

            return Ok();
        }

        [HttpGet("reports")]
        public ActionResult GetReports([FromQuery] ReportQuery query)
        {
            var result = _adminService.GetReports(query);

            return Ok(result);
        }

        [HttpPut("report/{id}")]
        public ActionResult UpdateReport([FromRoute] int id, [FromBody] UpdateReportDto dto)
        {
            _adminService.UpdateReport(id, dto);

            return Ok();
        }
    }
}
