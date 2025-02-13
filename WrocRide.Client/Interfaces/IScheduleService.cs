using WrocRide.Shared.DTOs.Schedule;
using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.Client.Interfaces
{
    public interface IScheduleService
    {
        Task<ScheduleDto> GetScheduleById(int id);
        Task<PagedList<ScheduleDto>> GetAllSchedules(int pageSize, int pageNumber);
        Task DeleteSchedule(int id);
        Task CreateSchedule(CreateScheduleDto dto);
    }
}
