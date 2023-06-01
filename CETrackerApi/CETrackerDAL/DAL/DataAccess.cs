using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace CETrackerDAL.DAL;

public interface IDataAccess
{
	Task<IEnumerable<T>> LoadData<T, U>(
		string storedProcedure,
		U parameters,
		string connectionId = "Default"
	);
}

public class DataAccess : IDataAccess
{
	private readonly IConfiguration _config;

	public DataAccess(IConfiguration config)
	{
		_config = config;
	}

    public async Task<IEnumerable<T>> LoadData<T, U>(
		string storedProcedure,
		U parameters,
		string connectionId = "Default"
	)
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

        var result = await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

		return result;
    }
}
