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
    int UpdateExperience(UpdateExperienceRequest request, CancellationToken token);
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

    public int UpdateExperience(UpdateExperienceRequest updateExperienceRequest, CancellationToken cancellationToken)
    {
        using var txScope = new TransactionScope(TransactionScopeOption.RequiresNew);

        try
        {
            using DbConnection connection = _dataConnectionFactory.CeTrackerSqlConnection();
            connection.Open();

            var experienceId = UpdateExperience(updateExperienceRequest, connection);

            UpdateExperienceCategories(experienceId, updateExperienceRequest, connection);

            UpdateExperienceAmounts(experienceId, updateExperienceRequest, connection);
            
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

    internal virtual int UpdateExperience(UpdateExperienceRequest updateExperienceRequest, DbConnection connection)
    {
        var updateUserId = 0; // TODO: get the user id from the auth context
        var experienceId = connection.QuerySingle<int>("ce.Experiences_U_I",
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

    internal virtual void UpdateExperienceCategories(int experienceId, UpdateExperienceRequest request, IDbConnection conn)
    {
        var updateUserId = 0; // TODO: get the user id from the auth context

        foreach (var experienceCategory in request.Categories)
        {
            conn.Query("ce.ExperienceCategory_U_I",
                new
                {
                    experienceCategory.ExperienceCategoryId,
                    experienceId,
                    experienceCategory.CategoryId,
                    updateUserId
                }, commandType: CommandType.StoredProcedure);
        }
    }

    internal virtual void UpdateExperienceAmounts(int experienceId, UpdateExperienceRequest request, IDbConnection conn)
    {
        var updateUserId = 0; // TODO: get the user id from the auth context

        foreach (var experienceAmount in request.Amounts)
        {
            conn.Query("ce.ExperienceAmount_U_I",
                new
                {
                    experienceAmount.ExperienceAmountId,
                    experienceId,
                    experienceAmount.UnitId,
                    experienceAmount.Amount,
                    updateUserId
                }, commandType: CommandType.StoredProcedure);
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
