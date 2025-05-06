using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;
        private readonly ISharedService _sharedService;

        public TripsController(ITripsService tripsService, ISharedService sharedService)
        {
            _tripsService = tripsService;
            _sharedService = sharedService;
        }

        /*
         * Retrieve all trips.
         * Includes: ID, name, description, date range, maximum number of participants and countries.
         */
        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _tripsService.GetTrips();
            return Ok(trips);
        }

        /*
         * Retrieve a single trip (not in the task).
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            if(await _sharedService.DoesTripExist(id) == false){
                  return NotFound();
            }
            var tripTask = _tripsService.GetTrip(id);
            return Ok(tripTask.Result);
        }
    }
}
