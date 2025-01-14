

namespace WrocRide.API.Controllers
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
        public async Task<ActionResult<UserDto>> Get()
        {
            var result = await _userService.GetUser();

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
        {
            await _userService.UpdateUser(dto);

            return Ok();
        }

        [HttpPut("balance")]
        public async Task<ActionResult> AddCredits([FromBody] AddCreditsDto dto)
        {
            await _userService.AddCredits(dto);
            
            return Ok();
        }

        [HttpPut("deactivate-account")]
        public async Task<ActionResult> DeactivateAccount()
        {
            await _userService.DeactivateAccount();

            return Ok();
        }
    }
}
