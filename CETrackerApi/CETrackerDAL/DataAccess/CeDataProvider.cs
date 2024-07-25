using System.Data;
using System.Transactions;
using DALModels = CETrackerDAL.Models;
using Dapper;
using CETracker.Contracts.DataContracts;
using CETracker.Contracts.RequestContracts;
using System.Data.SqlClient;

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

    public async Task UpdateExperience(UpdateExperienceRequest updateExperienceRequest)
    {
        using var txScope = new TransactionScope(TransactionScopeOption.RequiresNew);

        try
        {
            using IDbConnection connection = _dataConnectionFactory.CeTrackerSqlConnection();

            var experienceId = await connection.QuerySingleAsync<int>("ce.Experiences_U_I",
                new {
                    updateExperienceRequest.UserId, // TODO: get the user id from the auth context
                    updateExperienceRequest.LocationId,
                    updateExperienceRequest.CarryForward,
                    updateExperienceRequest.ProgramTitle,
                    updateExperienceRequest.EventName,
                    updateExperienceRequest.StartDate,
                    updateExperienceRequest.EndDate,
                    updateExperienceRequest.Description,
                    updateExperienceRequest.Notes
                }, (IDbTransaction)Transaction.Current);

            await UpdateExperienceCategories(experienceId, updateExperienceRequest, connection).ConfigureAwait(false);

            await UpdateExperienceAmounts(experienceId, updateExperienceRequest, connection).ConfigureAwait(false);

            txScope.Complete();
        }
        catch (SqlException e)
        {
            // TODO: logging
            throw;
        }
        finally
        {
            txScope.Dispose();
        }
    }

    internal virtual async Task UpdateExperienceCategories(int experienceId, UpdateExperienceRequest request, IDbConnection conn)
    {
        foreach (var experienceCategory in request.ExperienceCategories)
        {
            await conn.QueryAsync("ce.ExperienceCategory_U_I",
                new
                {
                    experienceCategory.ExperienceCategoryId,
                    experienceId,
                    experienceCategory.CategoryId,
                    request.UserId //TODO: get the user id from the auth context
                }, (IDbTransaction)Transaction.Current);
        }
    }

    internal virtual async Task UpdateExperienceAmounts(int experienceId, UpdateExperienceRequest request, IDbConnection conn)
    {
        foreach (var experienceAmount in request.ExperienceAmounts)
        {
            await conn.QueryAsync("ce.ExperienceAmount_U_I",
                new
                {
                    experienceAmount.ExperienceAmountId,
                    experienceId,
                    experienceAmount.UnitId,
                    experienceAmount.Amount,
                    request.UserId //TODO: get the user id from the auth context
                }, (IDbTransaction)Transaction.Current);
        }
    }

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
