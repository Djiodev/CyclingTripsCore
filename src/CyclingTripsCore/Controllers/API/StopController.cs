using System;
using CyclingTrips.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using AutoMapper;
using System.Collections.Generic;
using CyclingTrips.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CyclingTrips.Controllers.API
{

    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private ILogger _logger;
        private ICyclingTripsRepository _repository;

        public StopController(ICyclingTripsRepository repository, ILogger<StopController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try {
                var trip = _repository.GetTripByName(tripName, User.Identity.Name);

                if (trip == null)
                {
                    return Json(null);
                }
                
                return Json(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Arrival)));
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed to get stops for the trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("")]
        public JsonResult Post(string tripName, [FromBody] StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);

                    _repository.AddNewStop(tripName, User.Identity.Name, newStop);

                    if (_repository.saveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to save new stop");
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new stop");
        }

        [Authorize]
        [HttpDelete("{stopToDelete}")]
        public JsonResult delete(string tripName, string stopToDelete)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repository.deleteStop(tripName, stopToDelete);
                    if (_repository.saveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Accepted;
                        return Json("Stop Deleted");
                    }

                }

            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to delete the stop: " + ex);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Failed");
        }
    }
}
