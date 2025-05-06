using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface ITripsService
{
    Task<TripDTO> GetTrip(int tripId);
    Task<List<TripDTO>> GetTrips();
}