﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WrocRide.Entities;
using WrocRide.Helpers;
using WrocRide.Models;
using WrocRide.Services;

namespace WrocRide.Controllers
{
    [Authorize(Policy = "IsActivePolicy")]
    [Route("api/ride")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public ActionResult CreateRide([FromBody] CreateRideDto dto)
        {
            int id = _rideService.CreateRide(dto);

            return Created($"api/ride/{id}", null);
        }

        [HttpPost("reservation")]
        [Authorize(Roles = "Client")]
        public ActionResult CreateRideReservation([FromBody] CreateRideReservationDto dto)
        {
            int id = _rideService.CreateRideReservation(dto);

            return Created($"api/ride/{id}", null);
        }

        [HttpGet]
        public ActionResult<PagedList<RideDto>> GetAllRides([FromQuery] RideQuery query)
        {
            var result = _rideService.GetAll(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<RideDeatailsDto> GetRideById([FromRoute] int id)
        {
            var result = _rideService.GetById(id);

            return Ok(result);
        }

        [HttpPut("{id}/ride-status")]
        public ActionResult UpdateRideStatus([FromRoute] int id, [FromBody] UpdateRideStatusDto dto)
        {
            _rideService.UpdateRideStatus(id, dto);

            return Ok();
        }

        [HttpPut("{id}/driver-decision")]
        public ActionResult DriverDecision([FromRoute] int id, [FromBody] RideDriverDecisionDto dto)
        {
            _rideService.DriverDecision(id, dto);

            return Ok();
        }

        [HttpPut("{id}/cancel-ride")]
        public ActionResult CancelRide([FromRoute] int id)
        {
            _rideService.CancelRide(id);

            return Ok();
        }

        [HttpPut("{id}/end-ride")]
        public ActionResult EndRide([FromRoute] int id)
        {
            _rideService.EndRide(id);

            return Ok();
        }
    }
}