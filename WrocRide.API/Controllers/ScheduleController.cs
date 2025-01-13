namespace WrocRide.API.Controllers;

[Authorize(Roles = "Client")]
[Authorize(Policy = "IsActivePolicy")]
[Route("api/schedule")]
[ApiController]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    
    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateScheduleDto dto)
    {
        var id = _scheduleService.CreateSchedule(dto);

        return Created($"api/schedule/{id}", null);
    }
    
    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _scheduleService.DeleteSchedule(id);
        
        return NoContent();
    }

    [HttpGet("{id}")]
    public ActionResult<ScheduleDto> GetById([FromRoute] int id)
    {
        var schedule = _scheduleService.GetSchedule(id);
        
        return Ok(schedule);
    }
}