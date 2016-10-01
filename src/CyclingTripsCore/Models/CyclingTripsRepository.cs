using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CyclingTrips.Models
{
    public class CyclingTripsRepository : ICyclingTripsRepository
    {
        private CyclingTripsDbContext _context;

        public CyclingTripsRepository(CyclingTripsDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trips
                .OrderBy(t => t.Created)
                .ToList();
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            return _context.Trips
                .Include(t => t.Stops)
                .OrderBy(t => t.Created)
                .ToList();
        }

        public IEnumerable<Comment> GetAllCommentsForATrip(int tripID)
        {
            return _context.Comments
                .Where(t => t.TripID == tripID)
                .ToList();
        }

        public void AddTrip(Trip newTrip)
        {
            _context.Add(newTrip);
        }

        public bool saveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public Trip GetTripById(int tripId)
        {
            return _context.Trips
                    .Include(t => t.Stops)
                    .Include(t => t.Comments)
                    .Where(t => t.ID == tripId)
                    .FirstOrDefault();
        }

        public Trip GetTripByName(string tripName, string username)
        {
            return _context.Trips.Include(t => t.Stops)
                        .Include(t => t.Comments)
                        .Where(t => t.Name == tripName && t.Username == username)
                        .FirstOrDefault();
        }

        public void AddNewStop(string tripName, string username, Stop newStop)
        {
            var trip = GetTripByName(tripName, username);
            trip.Stops.Add(newStop);

            _context.Stops.Add(newStop);
        }

        
        public IEnumerable<Trip> GetUserTripsWithStops(string username)
        {
            return _context.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Departure)
                    .Where(t => t.Username == username)
                    .ToList();
        }

        public Stop getStopByNameAndTripName(string tripName, string stopToDelete)
        {
            var trip = _context.Trips
                        .Include(t => t.Stops)
                        .Where(t => t.Name == tripName)
                        .FirstOrDefault();
            var stop = trip.Stops
                        .Where(s => s.Location == stopToDelete)
                        .FirstOrDefault();
            return stop;
        }

        public void deleteStop(string tripName, string stopToDelete)
        {
            var stop = getStopByNameAndTripName(tripName, stopToDelete);
            _context.Remove(stop);
        }
        
        public void deleteTrip(string tripName, string username)
        {
            var tripToDelete = GetTripByName(tripName, username);

            _context.Trips.Remove(tripToDelete);

        }

        public IEnumerable<Trip> GetAllTripsWithStopsAndComments()
        {
            var results = _context.Trips
                            .Include(t => t.Stops)
                            .OrderBy(s => s.Departure)
                            .Include(t => t.Comments)
                            .ToList();
            return results;
        }

        public void addNewComment(Comment newComment, string tripUsername)
        {
            var trip = GetTripById(newComment.TripID);
            trip.Comments.Add(newComment);

            _context.Comments.Add(newComment);
        }

        public void deleteComment(int commentID)
        {
            var commentToDelete = _context.Comments
                                    .Where(c => c.ID == commentID)
                                    .FirstOrDefault();
            _context.Comments.Remove(commentToDelete);
        }

        public string GetProfileImage(string username)
        {
            var user = _context.Users
                            .Where(u => u.UserName == username)
                            .FirstOrDefault();
            return user.ProfileImage;
        }
       
    }
}
