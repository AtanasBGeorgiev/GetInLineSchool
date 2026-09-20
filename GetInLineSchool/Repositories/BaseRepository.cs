using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace GetInLineSchool.Repositories
{
    public class BaseRepository
    {
        protected readonly string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
            ?? throw new Exception("Connection string not found in environment variables.");

        protected DbConnection dbConnection => new SqlConnection(connectionString);
    }
}
