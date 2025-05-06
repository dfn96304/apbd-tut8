using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;
        private readonly ISharedService _sharedService;

        public ClientsController(IClientsService clientsService, ISharedService sharedService)
        {
            _clientsService = clientsService;
            _sharedService = sharedService;
        }
        
        /*
         * Return all trips for which the client is registered for.
         * Includes: IdTrip, name, description, registration date of the client and payment date for the client.
         * 
         * If the client ID does not exist, this endpoint returns 404 Not Found.
         * If there are no trips assigned to the client, this endpoint returns an empty list.
         * Otherwise, this endpoint returns 200 OK.
         */
        [HttpGet("{clientId}/trips")]
        public async Task<IActionResult> GetTripsForClient(int clientId)
        {
            if (await _sharedService.DoesClientExist(clientId) == false)
            {
                return NotFound($"Client with id {clientId} not found");
            }
            var trips = await _clientsService.GetTripsForClient(clientId);
            return Ok(trips);
        }

        /*
         * Add a new client.
         * Client details (FirstName, LastName, Email, Telephone, Pesel) must be sent in the request body.
         */
        [HttpPost]
        public async Task<IActionResult> NewClient([FromBody] ClientDTO clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var clientTask = _clientsService.NewClient(clientDto);
            return Created();
        }
        
        /*
         * Assign a client to a trip.
         * If the client ID or trip ID does not exist, this endpoint returns 404 Not Found.
         * Otherwise, this endpoint returns 200 OK.
         */
        [HttpPut("{clientId}/trips/{tripId}")]
        public async Task<IActionResult> NewTripForClient(int clientId, int tripId)
        {
            if (await _sharedService.DoesClientExist(clientId) == false)
            {
                return NotFound($"Client with id {clientId} not found");
            }

            if (await _sharedService.DoesTripExist(tripId) == false)
            {
                return NotFound($"Trip with id {tripId} not found");
            }
            
            var tripTask = _clientsService.NewTripForClient(clientId, tripId);
            return Ok();
        }
        
        /*
         * Remove a client from a trip.
         * If the client ID or trip ID does not exist, this endpoint returns 404 Not Found.
         * Otherwise, this endpoint returns 200 OK.
         */
        [HttpDelete("{clientId}/trips/{tripId}")]
        public async Task<IActionResult> DeleteTripForClient(int clientId, int tripId)
        {
            if (await _sharedService.DoesClientExist(clientId) == false)
            {
                return NotFound($"Client with id {clientId} not found");
            }

            if (await _sharedService.DoesTripExist(clientId) == false)
            {
                return NotFound($"Trip with id {tripId} not found");
            }
            var tripTask = _clientsService.DeleteTripForClient(clientId, tripId);
            return Ok();
        }
    }
}