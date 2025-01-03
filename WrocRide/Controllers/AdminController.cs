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

        [HttpPut("documents/{id}")]
        public ActionResult UpdateDocument([FromRoute] int id, [FromBody] UpdateDocumentDto dto)
        {
            _adminService.UpdateDocument(id, dto);

            return Ok();
        }

        [HttpGet("documents/{driverId}")]
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

        [HttpPut("users/{userId}")]
        public ActionResult UpdateUser([FromRoute] int userId, [FromBody] UpdateUserDto dto)
        {
            _adminService.UpdateUser(userId, dto);

            return Ok();
        }
    }
}
