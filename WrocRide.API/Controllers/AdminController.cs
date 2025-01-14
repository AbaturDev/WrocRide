namespace WrocRide.API.Controllers
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
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocuments([FromQuery] DocumentQuery query)
        {
            var result = await _adminService.GetDocuments(query);

            return Ok(result);
        }

        [HttpPut("document/{id}")]
        public async Task<ActionResult> UpdateDocument([FromRoute] int id, [FromBody] UpdateDocumentDto dto)
        {
            await _adminService.UpdateDocument(id, dto);

            return Ok();
        }

        [HttpGet("document/{driverId}")]
        public async Task<ActionResult<DocumentDto>> GetDocumentByDriverId([FromRoute] int driverId)
        {
            var result = await _adminService.GetDocumentByDriverId(driverId);

            return Ok(result);
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] UserQuery query)
        {
            var result = await _adminService.GetAll(query);

            return Ok(result);
        }

        [HttpPut("user/{userId}")]
        public async Task<ActionResult> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserDto dto)
        {
            await _adminService.UpdateUser(userId, dto);

            return Ok();
        }

        [HttpGet("reports")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetReports([FromQuery] ReportQuery query)
        {
            var result = await _adminService.GetReports(query);

            return Ok(result);
        }

        [HttpPut("report/{id}")]
        public async Task<ActionResult> UpdateReport([FromRoute] int id, [FromBody] UpdateReportDto dto)
        {
            await _adminService.UpdateReport(id, dto);

            return Ok();
        }
    }
}
