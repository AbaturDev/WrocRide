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
        public ActionResult Register([FromBody] RegisterUserDto dto)
        {
            _accountService.Register(dto);
            return Ok();
        }

        [HttpPost("register-driver")]
        public ActionResult RegisterDriver([FromBody] RegisterDriverDto dto)
        {
            _accountService.RegisterDriver(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto dto)
        {
            string token = _accountService.Login(dto);

            return Ok(token);
        }
    }
}
