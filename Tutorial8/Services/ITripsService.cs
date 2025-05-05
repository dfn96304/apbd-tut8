using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface ITripsService
{
    Task<bool> DoesTripExist(int tripId);
    Task<TripDTO> GetTrip(int tripId);
    Task<List<TripDTO>> GetTrips();
}