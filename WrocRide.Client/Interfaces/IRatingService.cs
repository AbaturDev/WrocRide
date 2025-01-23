using WrocRide.Shared.DTOs.Rating;

namespace WrocRide.Client.Interfaces
{
    public interface IRatingService
    {
        Task<RatingDto> GetRating(int rideId);
        Task CreateRating(int rideId, CreateRatingDto dto);
        Task DeleteRating(int rideId);
        Task UpdateRating(int rideId, CreateRatingDto dto);
    }
}
