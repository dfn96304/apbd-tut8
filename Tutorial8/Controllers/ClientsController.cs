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

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        [HttpGet("{clientId}/trips")]
        public async Task<IActionResult> GetTripsForClient(int clientId)
        {
            var trips = await _clientsService.GetTripsForClient(clientId);
            return Ok(trips);
        }

        /*[HttpPost]
        public async Task<IActionResult> NewClient(ClientDTO clientDto)
        {
            
        }
        
        [HttpPut("${clientId}/trips/{tripId}")]
        public async Task<IActionResult> NewTripForClient(int clientId, int tripId)
        {
            
        }
        
        [HttpDelete("${clientId}/trips/{tripId}")]
        public async Task<IActionResult> DeleteTripForClient(int clientId, int tripId)
        {
            
        }*/
    }
}