using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";

    public async Task<TripDTO> parseTrip(SqlDataReader reader)
    {
        int idOrdinal = reader.GetOrdinal("IdTrip");
                        int nameOrdinal = reader.GetOrdinal("Name");
                        int descriptionOrdinal = reader.GetOrdinal("Description");
                        int dateFromOrdinal = reader.GetOrdinal("DateFrom");
                        int dateToOrdinal = reader.GetOrdinal("DateTo");
                        int maxPeopleOrdinal = reader.GetOrdinal("MaxPeople");

                        TripDTO newTrip = new TripDTO()
                        {
                            IdTrip = reader.GetInt32(idOrdinal),
                            Name = reader.GetString(nameOrdinal),
                            Description = reader.GetString(descriptionOrdinal),
                            DateFrom = reader.GetDateTime(dateFromOrdinal),
                            DateTo = reader.GetDateTime(dateToOrdinal),
                            MaxPeople = reader.GetInt32(maxPeopleOrdinal),
                            Countries = new List<CountryDTO>()
                        };

                        var commandCountries =
                            @"SELECT * FROM Country WHERE IdCountry IN (SELECT IdCountry FROM Country_Trip WHERE IdTrip = @IdTrip)";
                        
                        using (SqlConnection connCountries = new SqlConnection(_connectionString))
                        using (SqlCommand cmdCountries = new SqlCommand(commandCountries, connCountries))
                        {
                            cmdCountries.Parameters.AddWithValue("@IdTrip", newTrip.IdTrip);
                            
                            await connCountries.OpenAsync();

                            using (var readerCountries = await cmdCountries.ExecuteReaderAsync())
                            {
                                while (await readerCountries.ReadAsync())
                                {
                                    int idOrdinalCountries = readerCountries.GetOrdinal("IdCountry");
                                    int nameOrdinalCountries = readerCountries.GetOrdinal("Name");

                                    newTrip.Countries.Add(new CountryDTO()
                                    {
                                        IdCountry = readerCountries.GetInt32(idOrdinalCountries),
                                        Name = readerCountries.GetString(nameOrdinalCountries)
                                    });
                                }
                            }
                        }
        return newTrip;
    }
    
    public async Task<List<TripDTO>> GetTrips()
    {
        var trips = new List<TripDTO>();

        string command = "SELECT * FROM Trip";

        using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(command, conn))
            {
                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        TripDTO newTrip = await parseTrip(reader);
                        trips.Add(newTrip);
                    }
                }
            }
        
        return trips;
    }

    public async Task<TripDTO> GetTrip(int tripId)
    {
        TripDTO? newTrip = null;

        string command = @"SELECT * FROM Trip WHERE IdTrip = @IdTrip";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdTrip", tripId);
            
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    newTrip = await parseTrip(reader);
                }
            }
        }
        
        return newTrip;
    }
}