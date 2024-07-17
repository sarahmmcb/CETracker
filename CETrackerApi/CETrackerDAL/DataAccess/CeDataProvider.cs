using DALModels = CETrackerDAL.Models;
using System.Data;
using Dapper;
using CETracker.Contracts.DataContracts;

namespace CETrackerDAL.DataAccess;

public interface ICeDataProvider
{
    Task<IEnumerable<DALModels.Experience>> GetExperiencesByYear(int year, int userId, int nationalStandardId);
    Task<IEnumerable<DALModels.CategoryList>> GetCategoryLists(int year, int userId);
    Task<IEnumerable<Location>> GetLocations();
    Task<IEnumerable<Unit>> GetUnits(int nationalStandardId);
}

public class CeDataProvider : ICeDataProvider
{
    private readonly IDataConnectionFactory _dataConnectionFactory;

    public CeDataProvider(IDataConnectionFactory dataConnectionFactory)
    {
        _dataConnectionFactory = dataConnectionFactory;
    }

    public Task<IEnumerable<DALModels.Experience>> GetExperiencesByYear(int year, int userId, int nationalStandardId) =>
        LoadData<DALModels.Experience, dynamic>(
               "ce.Experiences_S",
            new
            {
                Year = year,
                UserId = userId,
                NationalStandardId = nationalStandardId
            }
        );

    public Task<IEnumerable<DALModels.CategoryList>> GetCategoryLists(int nationalStandardId, int year) =>
        LoadData<DALModels.CategoryList, dynamic>(
            "ce.CategoryLists_S"
            ,
            new
            {
                nationalStandardId,
                year
            }
        );

    public Task<IEnumerable<Location>> GetLocations() =>
        LoadData<Location, dynamic>(
            "ce.Locations_S",
            new {}
        );

    public Task<IEnumerable<Unit>> GetUnits(int nationalStandardId) => 
        LoadData<Unit, dynamic>(
               "ce.Units_S",
               new
               {
                   nationalStandardId
               });

    internal virtual async Task<IEnumerable<T>> LoadData<T, U>(
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
