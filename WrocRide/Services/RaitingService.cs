using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;
using WrocRide.Models;
using WrocRide.Exceptions;
using WrocRide.Models.Enums;

namespace WrocRide.Services
{
    public interface IRatingService
    {
        int CreateRating(int rideId, CreateRatingDto dto);
        RatingDto Get(int rideId);
        void Update(int rideId, CreateRatingDto dto);
        void Delete(int rideId);
    }

    public class RatingService : IRatingService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IUserContextService _userContext;
        public RatingService(WrocRideDbContext dbContext, IUserContextService userContext)
        {
            _dbContext = dbContext;
            _userContext = userContext;
        }

        public int CreateRating(int rideId, CreateRatingDto dto)
        {
            var ride = GetCurrentClientRide(rideId);

            if(ride.Rating != null)
            {
                throw new BadRequestException("Ride already have an rating");
            }

            var raiting = new Rating()
            {
                CreatedAt = DateTime.Now,
                Grade = dto.Grade,
                Comment = dto.Comment,
                RideId = ride.Id,
                CreatedByClientId = ride.ClientId
            };

            _dbContext.Ratings.Add(raiting);
            _dbContext.SaveChanges();

            return raiting.Id;
        }

        public void Update(int rideId, CreateRatingDto dto)
        {
            var ride = GetCurrentClientRide(rideId);

            if(ride.Rating == null)
            {
                throw new BadRequestException("Ride does not have a rating");
            }

            if(ride.Rating.CreatedByClientId != ride.ClientId)
            {
                throw new ForbidException("Client is not a rating creator");
            }

            ride.Rating.Grade = dto.Grade;
            ride.Rating.Comment = dto.Comment;

            _dbContext.SaveChanges();
        }

        public void Delete(int rideId)
        {
            var ride = GetCurrentClientRide(rideId);

            if(ride.Rating == null)
            {
                throw new BadRequestException("Ride does not have a rating");
            }

            if (ride.Rating.CreatedByClientId != ride.ClientId)
            {
                throw new ForbidException("Client is not a rating creator");
            }

            _dbContext.Ratings.Remove(ride.Rating);
            _dbContext.SaveChanges();
        }

        public RatingDto Get(int rideId)
        {
            var ride = _dbContext.Rides
                .Include(r => r.Rating)
                .FirstOrDefault(r => r.Id == rideId && r.RideStatus == RideStatus.Ended);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            if(ride.Rating == null)
            {
                throw new BadRequestException("Ride does not have a rating");
            }

            var result = new RatingDto()
            {
                Grade = ride.Rating.Grade,
                Comment = ride.Rating.Comment,
                CreatedAt = ride.Rating.CreatedAt
            };

            return result;
        }

        private Ride GetCurrentClientRide(int rideId)
        {
            var userId = _userContext.GetUserId;

            var client = _dbContext.Clients
                .Include(c => c.User)
                .FirstOrDefault(c => c.UserId == userId);

            if (client == null)
            {
                throw new BadRequestException("User is not a client");
            }

            var ride = _dbContext.Rides
                .Include(r => r.Rating)
                .FirstOrDefault(r => r.ClientId == client.Id && r.Id == rideId && r.RideStatus == RideStatus.Ended);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            return ride;
        }
    }
}
