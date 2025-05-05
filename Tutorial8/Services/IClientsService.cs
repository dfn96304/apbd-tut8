using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientsService
{
    Task<List<TripDTO>> GetTripsForClient(int clientId);
    //Task NewClient();
    //Task NewTripForClient(int clientId, int tripId);
    //Task DeleteTripForClient(int clientId, int tripId);
}