using System.Data;
using System.Data.Common;
using System.Transactions;
using DALModels = CETrackerDAL.Models;
using Dapper;
using CETracker.Contracts.DataContracts;
using CETracker.Contracts.RequestContracts;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CETrackerDAL.DataAccess;

public interface ICeDataProvider
{
    Task<IEnumerable<DALModels.Experience>> GetExperiencesByYear(int year, int userId, int nationalStandardId);
    Task<IEnumerable<DALModels.Experience>> GetExperienceById(int experienceId);
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

    public Task<IEnumerable<DALModels.Experience>> GetExperienceById(int experienceId) =>
        LoadData<DALModels.Experience, dynamic>(
               "ce.Experiences_S_By_Id",
            new
            {
                experienceId
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
        using var txScope = new TransactionScope(TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        using DbConnection connection = _dataConnectionFactory.CeTrackerSqlConnection();
        await connection.OpenAsync();

        try
        {
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
        catch (Exception e)
        {
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
                   StartDate = updateExperienceRequest.StartDate.ToUniversalTime().ToString(),
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

        var currentCategories = await LoadData<ExperienceCategory, dynamic>(
            "ce.ExperienceCategory_S",
            new
            {
                experienceId
            }, conn);

        var currentCatIds = currentCategories.Select(c => c.CategoryId);

        var categoriesToDelete = currentCategories.Where(cat => !request.Categories.Contains(cat.CategoryId));
        var categoriesToCreate = request.Categories.Where(catId => !currentCatIds.Contains(catId));

        foreach(var oldCategory in categoriesToDelete)
        {
            await conn.ExecuteAsync("ce.ExperienceCategory_D", new
            {
                experienceId,
                oldCategory.CategoryId,
                updateUserId
            }, commandType: CommandType.StoredProcedure);
        }

        foreach(var newCategoryId in categoriesToCreate)
        {
            if (newCategoryId == 0)
                continue;

            await conn.ExecuteAsync("ce.ExperienceCategory_I", new
            {
                experienceId,
                CategoryId = newCategoryId,
                updateUserId
            }, commandType: CommandType.StoredProcedure);
        }

    }

    internal virtual async Task UpdateExperienceAmounts(int experienceId, UpdateExperienceRequest request, IDbConnection conn)
    {
        var updateUserId = 0; // TODO: get the user id from the auth context

        var currentExperienceAmount = await LoadData<DALModels.ExperienceAmount, dynamic>(
            "ce.ExperienceAmount_S",
            new
            {
                experienceId
            }, conn);

        if (currentExperienceAmount == null || currentExperienceAmount.Count() == 0)
        {
            await UpdateExperienceAmount(experienceId, request.TimeSpentParent, updateUserId, conn);
            await UpdateExperienceAmount(experienceId, request.TimeSpentChild, updateUserId, conn);
        }
        else
        {
            var currentParentAmount = currentExperienceAmount.Where(am => am.ParentUnitId == 0).ToList().FirstOrDefault();
            var currentChildAmount = currentExperienceAmount.Where(am => am.ParentUnitId != 0).ToList().FirstOrDefault();

            if (request.TimeSpentParent.Amount != currentParentAmount.Amount)
                await UpdateExperienceAmount(experienceId, request.TimeSpentParent, updateUserId, conn);

            if (request.TimeSpentChild.Amount != currentChildAmount.Amount)
                await UpdateExperienceAmount(experienceId, request.TimeSpentChild, updateUserId, conn);
        }
    }

    private async Task UpdateExperienceAmount(int experienceId, ExperienceAmount amount, int updateUserId, IDbConnection conn)
    {
        await conn.ExecuteAsync(
            "ce.ExperienceAmount_U_I",
            new { 
                experienceId,
                amount.UnitId,
                amount.Amount,
                updateUserId
            }, commandType: CommandType.StoredProcedure);
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

    internal virtual async Task<IEnumerable<T>> LoadData<T, U>(
    string storedProcedure,
    U parameters,
    IDbConnection connection
)
    {
        var result = await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }
}
