using WrocRide.Shared.DTOs.Driver;
using WrocRide.Shared.DTOs.Rating;
using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.Client.Interfaces
{
    public interface IDriverService
    {
        Task<PagedList<DriverDto>> GetAll(int pageSize, int pageNumber);
        Task<DriverDto> GetById(int id);
        Task<PagedList<RatingDto>> GetRatings(int id, int pageSize, int pageNumber);
        Task UpdatePricing(UpdateDriverPricingDto dto);
        Task UpdateDriverStatus(UpdateDriverStatusDto dto);
    }
}
