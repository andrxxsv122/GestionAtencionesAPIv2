using System.Data;
using System.Data.SqlClient;

namespace GestionAtencionesAPI.Data
{
    public class DbContext
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public DbContext(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

    }
}
