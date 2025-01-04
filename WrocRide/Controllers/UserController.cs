using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WrocRide.Entities;
using WrocRide.Models;
using WrocRide.Services;

namespace WrocRide.Controllers
{
    [Authorize(Policy = "IsActivePolicy")]
    [ApiController]
    [Route("api/me")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<UserDto> Get()
        {
            var result = _userService.GetUser();

            return Ok(result);
        }

        [HttpPut]
        public ActionResult UpdateProfile([FromBody] UpdateUserDto dto)
        {
            _userService.UpdateUser(dto);

            return NoContent();
        }

        [HttpPut("balance")]
        public ActionResult AddCredits([FromBody] AddCreditsDto dto)
        {
            _userService.AddCredits(dto);
            
            return Ok();
        }

        [HttpPut("deactivate-account")]
        public ActionResult DeactivateAccount()
        {
            _userService.DeactivateAccount();

            return Ok();
        }
    }
}
