using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class ClientsService : IClientsService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
    
    public async Task<List<TripDTO>> GetTripsForClient(int clientId)
    {
        var trips = new List<TripDTO>();
        
        string command = $"SELECT * FROM Trip WHERE IdTrip IN (SELECT IdTrip FROM Client_Trip WHERE IdClient = {clientId})";
        
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
                    
                    trips.Add(new TripDTO()
                    {
                        IdTrip = reader.GetInt32(idOrdinal),
                        Name = reader.GetString(nameOrdinal)
                    });
                }
            }
        }

        return trips;
    }
    
    public async Task NewClient(ClientDTO clientDto)
    {
        string command = $"INSERT INTO Client (IdClient, FirstName, LastName, Email, Telephone, Pesel) VALUES ({clientDto.IdClient}, {clientDto.FirstName}, {clientDto.LastName}, {clientDto.Email}, {clientDto.Telephone}, {clientDto.Pesel})";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();
            
            cmd.ExecuteNonQuery();
        }
    }

    public async Task NewTripForClient(int clientId, int tripId)
    {
        string command = $"INSERT INTO Client_Trip (IdClient, TripId) VALUES ({clientId}, {tripId})";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();
            
            cmd.ExecuteNonQuery();
        }
    }

    public async Task DeleteTripForClient(int clientId, int tripId)
    {
        string command = $"DELETE FROM Client_Trip (IdClient, TripId) WHERE IdClient = {clientId} AND IdTrip = {tripId}";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();
            
            cmd.ExecuteNonQuery();
        }
    }
}