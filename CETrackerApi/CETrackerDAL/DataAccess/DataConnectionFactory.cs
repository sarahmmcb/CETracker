using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CETrackerDAL.DataAccess;

public interface IDataConnectionFactory
{
    DbConnection CeTrackerSqlConnection();
}
public class DataConnectionFactory : IDataConnectionFactory
{
    private readonly IConfiguration _config;

    public DataConnectionFactory(IConfiguration config)
    {
        _config = config;
    }

    public DbConnection CeTrackerSqlConnection() => new SqlConnection(_config.GetSection("CeTrackerSqlConnection").Value);
}
