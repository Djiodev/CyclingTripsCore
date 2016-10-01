using System.Collections.Generic;

namespace CyclingTrips.Models
{
    public interface ICyclingTripsRepository
    {
        IEnumerable<Comment> GetAllCommentsForATrip(int tripID);
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithStops();
        void AddTrip(Trip newTrip);
        bool saveAll();
        Trip GetTripByName(string tripName, string username);
        void AddNewStop(string tripName, string username, Stop newStop);
        IEnumerable<Trip> GetUserTripsWithStops(string username);
        void deleteStop(string tripName, string stopName);
        void deleteTrip(string tripName, string username);
        IEnumerable<Trip> GetAllTripsWithStopsAndComments();
        Trip GetTripById(int id);
        void addNewComment(Comment newComment, string tripUsername);
        void deleteComment(int commentID);
        string GetProfileImage(string username);
    }
}