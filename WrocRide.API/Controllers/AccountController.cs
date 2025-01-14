namespace WrocRide.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto dto)
        {
            await _accountService.Register(dto);
            return Ok();
        }

        [HttpPost("register-driver")]
        public async Task<ActionResult> RegisterDriver([FromBody] RegisterDriverDto dto)
        {
            await _accountService.RegisterDriver(dto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserDto dto)
        {
            string token = await _accountService.Login(dto);

            return Ok(token);
        }
    }
}
