using AutoMapper;
using CyclingTrips.Models;
using CyclingTrips.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CyclingTrips.Controllers.API
{
    [Route("api/trips")]
    public class TripController : Controller
    {
        private ILogger<Trip> _logger;
        private ICyclingTripsRepository _repository;

        public TripController(ICyclingTripsRepository repository, ILogger<Trip> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        //[HttpGet("")]
        //public JsonResult GetUnauthorized()
        //{
        //    var trips = Mapper.Map<IEnumerable<TripViewModel>>(_repository.GetAllTripsWithStops());
        //    return Json(trips);
        //}

        [HttpGet("")]
        [Authorize]
        public JsonResult Get()
        {
            var trips = _repository.GetUserTripsWithStops(User.Identity.Name);
            var results = Mapper.Map<IEnumerable<TripViewModel>>(trips);

            return Json(results);
        }

        [Authorize]
        [HttpPost("")]
        public JsonResult Post([FromBody]TripViewModel newTripVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTrip = Mapper.Map<Trip>(newTripVm);

                    newTrip.Username = User.Identity.Name;

                    _logger.LogInformation("Saving the new trip");
                    _repository.AddTrip(newTrip);

                    if (_repository.saveAll())
                    {

                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<TripViewModel>(newTrip));

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save New Trip.", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed.", ModelState = ModelState });
        }


        [Authorize]
        [HttpDelete("{tripName}")]
        public JsonResult delete(string tripName)
        {
            try
            {
                _logger.LogInformation("Removing the trip");
                var username = User.Identity.Name;

                _repository.deleteTrip(tripName, username);

                if (_repository.saveAll())
                {

                    Response.StatusCode = (int)HttpStatusCode.Accepted;
                    return Json("Trip Deleted Successfully");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete the trip.", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed.", ModelState = ModelState });


        }

        [Authorize]
        [HttpGet("{tripName}/comments")]
        public JsonResult GetComments(string tripName)
        {
            var trip = _repository.GetTripByName(tripName, User.Identity.Name);
            var comments = _repository.GetAllCommentsForATrip(trip.ID);
            var results = AutoMapper.Mapper.Map<IEnumerable<CommentViewModel>>(comments);
            foreach(var c in results)
            {
                c.UserProfileImg = _repository.GetProfileImage(c.Username);
            }

            return Json(results);
        }

        [Authorize]
        [HttpDelete("{tripName}/comments/{commentID}")]
        public JsonResult DeleteComment(string tripName, int commentID)
        {
            try
            {
                _logger.LogInformation("Removing the comment");

                _repository.deleteComment(commentID);

                if (_repository.saveAll())
                {

                    Response.StatusCode = (int)HttpStatusCode.Accepted;
                    return Json("Comment Deleted Successfully");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete the comment.", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed.", ModelState = ModelState });


        }

    }
}
