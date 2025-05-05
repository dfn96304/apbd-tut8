using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class ClientsService : IClientsService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD;Integrated Security=True;";
    
    public async Task<List<TripDTO>> GetTripsForClient(int clientId)
    {
        var trips = new List<TripDTO>();
        
        string command = $"SELECT * FROM Trips WHERE IdTrip IN (SELECT IdTrip FROM Client_Trip WHERE IdClient = {clientId})";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("IdTrip");
                    int nameOrdinal = reader.GetOrdinal("NameTrip");
                }
            }
        }

        return trips;
    }

    /*public Task NewClient()
    {
        
    }

    public Task NewTripForClient(int clientId, int tripId)
    {
        
    }

    public Task DeleteTripForClient(int clientId, int tripId)
    {
        
    }*/
}