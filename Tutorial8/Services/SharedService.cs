using Microsoft.Data.SqlClient;

namespace Tutorial8.Services;

public class SharedService : ISharedService
{
    private readonly string _connectionString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";

    public async Task<bool> DoesClientExist(int id)
    {
        bool doesExist = false;

        string command = @"SELECT COUNT(*) AS CountWithId FROM Client WHERE IdClient = @id";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);

            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("CountWithId");

                    int countWithId = reader.GetInt32(idOrdinal);

                    if (countWithId > 0) doesExist = true;
                    else doesExist = false;
                }
            }
        }

        return doesExist;
    }

    public async Task<bool> DoesTripExist(int id)
    {
        bool doesExist = false;

        string command = @"SELECT COUNT(*) AS CountWithId FROM Trip WHERE IdTrip = @id";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);

            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("CountWithId");

                    int countWithId = reader.GetInt32(idOrdinal);

                    if (countWithId > 0) doesExist = true;
                    else doesExist = false;
                }
            }
        }

        return doesExist;
    }
}