using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class ClientsService : IClientsService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
    
    public async Task<List<Client_TripDTO>> GetTripsForClient(int clientId)
    {
        var trips = new List<Client_TripDTO>();
        
        string command = @"SELECT * FROM Trip INNER JOIN Client_Trip ON Trip.IdTrip = Client_Trip.IdTrip WHERE IdClient = @clientId";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@clientId", clientId);
            
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("IdTrip");
                    int nameOrdinal = reader.GetOrdinal("Name");
                    int descriptionOrdinal = reader.GetOrdinal("Description");
                    
                    int registeredAtOrdinal = reader.GetOrdinal("RegisteredAt");
                    int paymentDateOrdinal = reader.GetOrdinal("PaymentDate");
                    
                    trips.Add(new Client_TripDTO()
                    {
                        IdTrip = reader.GetInt32(idOrdinal),
                        Name = reader.GetString(nameOrdinal),
                        Description = reader.GetString(descriptionOrdinal),
                        RegisteredAt = reader.GetInt32(paymentDateOrdinal),
                        PaymentDate = reader.GetInt32(paymentDateOrdinal)
                    });
                }
            }
        }

        return trips;
    }
    
    public async Task NewClient(ClientDTO clientDto)
    {
        string command = @"INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel) VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel)";
        
        Console.WriteLine(command);
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient",   clientDto.IdClient);
            cmd.Parameters.AddWithValue("@FirstName",  clientDto.FirstName);
            cmd.Parameters.AddWithValue("@LastName",   clientDto.LastName);
            cmd.Parameters.AddWithValue("@Email",      clientDto.Email);
            cmd.Parameters.AddWithValue("@Telephone",  clientDto.Telephone);
            cmd.Parameters.AddWithValue("@Pesel",      clientDto.Pesel);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task NewTripForClient(int clientId, int tripId)
    {
        DateTime currentDate = DateTime.Now;
        
        string command = @"INSERT INTO Client_Trip (IdClient, TripId, RegisteredAt) VALUES (@clientId, @tripId, @currentDate)";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient",   clientId);
            cmd.Parameters.AddWithValue("@TripId", tripId);
            cmd.Parameters.AddWithValue("@currentDate", currentDate);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteTripForClient(int clientId, int tripId)
    {
        string command = @"DELETE FROM Client_Trip (IdClient, TripId) WHERE IdClient = @clientId AND IdTrip = @tripId";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient",   clientId);
            cmd.Parameters.AddWithValue("@TripId", tripId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}