using WrocRide.Shared.PaginationHelpers;

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
    public async Task<ActionResult> Create([FromBody] CreateScheduleDto dto)
    {
        var id = await _scheduleService.CreateSchedule(dto);

        return Created($"api/schedule/{id}", null);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await _scheduleService.DeleteSchedule(id);
        
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleDto>> GetById([FromRoute] int id)
    {
        var schedule = await _scheduleService.GetSchedule(id);
        
        return Ok(schedule);
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<ScheduleDto>>> GetAll([FromQuery] PageQuery query)
    {
        var schedules = await _scheduleService.GetAll(query);

        return Ok(schedules);
    }
}