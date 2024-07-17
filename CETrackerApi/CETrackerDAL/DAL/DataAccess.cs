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
	private readonly IDataConnectionFactory _dataConnectionFactory;

	public DataAccess(IDataConnectionFactory dataConnectionFactory)
	{
		_dataConnectionFactory = dataConnectionFactory;
	}

    public async Task<IEnumerable<T>> LoadData<T, U>(
		string storedProcedure,
		U parameters,
		string connectionId = "Default"
	)
    {
        using IDbConnection connection = _dataConnectionFactory.CeTrackerSqlConnection();

        var result = await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

		return result;
    }
}
