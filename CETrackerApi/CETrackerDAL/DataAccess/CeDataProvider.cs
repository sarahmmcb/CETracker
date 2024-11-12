using System.Data;
using System.Data.Common;
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
    Task<int> UpdateExperience(UpdateExperienceRequest request, CancellationToken token);
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

    public async Task<int> UpdateExperience(UpdateExperienceRequest updateExperienceRequest, CancellationToken cancellationToken)
    {
        using var txScope = new TransactionScope(TransactionScopeOption.RequiresNew);

        try
        {
            using DbConnection connection = _dataConnectionFactory.CeTrackerSqlConnection();
            connection.Open();

            var experienceId = await UpdateExperience(updateExperienceRequest, connection);

            await UpdateExperienceCategories(experienceId, updateExperienceRequest, connection);

            await UpdateExperienceAmounts(experienceId, updateExperienceRequest, connection);
            
            txScope.Complete();

            return experienceId;
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

    internal virtual async Task<int> UpdateExperience(UpdateExperienceRequest updateExperienceRequest, IDbConnection connection)
    {
        var updateUserId = 0; // TODO: get the user id from the auth context
        var experienceId = await connection.QuerySingleAsync<int>("ce.Experiences_U_I",
               new
               {
                   updateExperienceRequest.ExperienceId,
                   updateExperienceRequest.UserId,
                   updateExperienceRequest.LocationId,
                   updateExperienceRequest.CarryForward,
                   updateExperienceRequest.ProgramTitle,
                   updateExperienceRequest.EventName,
                   updateExperienceRequest.StartDate,
                   updateExperienceRequest.EndDate,
                   updateExperienceRequest.Description,
                   updateExperienceRequest.Notes,
                   updateUserId 
               }, commandType: CommandType.StoredProcedure);

        return experienceId;
    }

    internal virtual async Task UpdateExperienceCategories(int experienceId, UpdateExperienceRequest request, IDbConnection conn)
    {
        var updateUserId = 0; // TODO: get the user id from the auth context

        if (request.Categories.Length == 0)
        {
            return;
        }

        // Don't do this, better to query all existing records and do a compare
        // then update based on that
        await conn.QueryAsync("ce.ExperienceCategory_D");

        foreach (var experienceCategoryId in request.Categories)
        {
           await conn.QueryAsync("ce.ExperienceCategory_I",
                new
                {
                    experienceId,
                    experienceCategoryId,
                    updateUserId
                }, commandType: CommandType.StoredProcedure);
        }
    }

    internal virtual async Task UpdateExperienceAmounts(int experienceId, UpdateExperienceRequest request, IDbConnection conn)
    {
        var updateUserId = 0; // TODO: get the user id from the auth context

        await Task.Delay(100);

        //foreach (var experienceAmount in request.Amounts)
        //{
        //   await conn.QueryAsync("ce.ExperienceAmount_U_I",
        //        new
        //        {
        //            experienceId,
        //            experienceAmount.UnitId,
        //            experienceAmount.Amount,
        //            updateUserId
        //        }, commandType: CommandType.StoredProcedure);
        //}
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
