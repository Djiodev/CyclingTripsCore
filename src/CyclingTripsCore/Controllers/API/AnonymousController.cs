using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CyclingTrips.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Net;
using CyclingTrips.ViewModels;
using AutoMapper;

namespace CyclingTrips.Controllers
{

    [Route("api/allTrips")]
    public class AnonymousController : Controller
    {
        private ICyclingTripsRepository _repository;

        public AnonymousController(ICyclingTripsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("")]
        public JsonResult GetAllTrips()
        {
            var trips = _repository.GetAllTrips();

            return Json(trips);
        }

        [HttpGet("{tripName}/{username}/stops")]
        public JsonResult GetAllStops(string tripName, string username)
        {
            var trip = _repository.GetAllTripsWithStopsAndComments()
                            .Where(t => t.Name == tripName && t.Username == username)
                            .FirstOrDefault();
            return Json(trip.Stops.OrderBy(s => s.Arrival).ToList());
        }

        [HttpGet("{tripName}/{username}/comments")]
        public JsonResult GetAllComments(string tripName, string username)
        {
            var trip = _repository.GetAllTripsWithStopsAndComments()
                            .Where(t => t.Name == tripName && t.Username == username)
                            .FirstOrDefault();
            return Json(trip.Comments.OrderBy(c => c.Created).ToList());
        }

        [Authorize]
        [HttpPost("{tripName}/{username}/comments")]
        public JsonResult Post(string tripName, string username, [FromBody]string comment)
        {            
            try
            {
                if (ModelState.IsValid)
                {
                    var newComment = new Comment()
                    {
                        Username = User.Identity.Name,
                        TripID = _repository.GetTripByName(tripName, username).ID,
                        Body = comment,
                        Created = DateTime.Now
                    };
                    _repository.addNewComment(newComment, username);
                    if (_repository.saveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<CommentViewModel>(newComment));
                    }

                }
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to save new comment");
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new comment");


        }
    }


}