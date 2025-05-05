using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";

    public async Task<bool> DoesTripExist(int id)
    {
        bool doesTripExist = false;

        string command = $"SELECT COUNT(*) AS CountOfTripsWithId FROM Trip WHERE IdTrip = {id}";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("CountOfTripsWithId");
                    
                    int countOfTripsWithId = reader.GetInt32(idOrdinal);
                    
                    if(countOfTripsWithId > 0) doesTripExist = true;
                    else doesTripExist = false;
                }
            }
        }
        
        return doesTripExist;
    }

    public async Task<List<TripDTO>> GetTrips()
    {
        return await GetTrips(null);
    }

    public async Task<TripDTO> GetTrip(int tripId)
    {
        var trips = await GetTrips(tripId);
        return trips.FirstOrDefault();
    }

    private async Task<List<TripDTO>> GetTrips(int? tripId)
    {
        var trips = new List<TripDTO>();

        string command = "SELECT * FROM Trip" + (tripId == null ? "" : $" WHERE IdTrip = {tripId}");
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("IdTrip");
                    int nameOrdinal = reader.GetOrdinal("Name");
                    int descriptionOrdinal = reader.GetOrdinal("Description");
                    int dateFromOrdinal = reader.GetOrdinal("DateFrom");
                    int dateToOrdinal = reader.GetOrdinal("DateTo");
                    int maxPeopleOrdinal = reader.GetOrdinal("MaxPeople");
                    
                    trips.Add(new TripDTO()
                    {
                        Id = reader.GetInt32(idOrdinal),
                        Name = reader.GetString(nameOrdinal),
                        Description = reader.GetString(descriptionOrdinal),
                        DateFrom = reader.GetDateTime(dateFromOrdinal),
                        DateTo = reader.GetDateTime(dateToOrdinal),
                        MaxPeople = reader.GetInt32(maxPeopleOrdinal)
                    });
                }
            }
        }
        
        return trips;
    }
}