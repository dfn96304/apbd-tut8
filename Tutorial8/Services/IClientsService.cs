using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientsService
{
    Task<List<Client_TripDTO>> GetTripsForClient(int clientId);
    Task NewClient(ClientDTO clientDto);
    Task NewTripForClient(int clientId, int tripId);
    Task DeleteTripForClient(int clientId, int tripId);
}