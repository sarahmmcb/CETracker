using System.Data;
using System.Data.Common;
using System.Transactions;
using DALModels = CETrackerDAL.Models;
using Dapper;
using CETracker.Contracts.DataContracts;
using CETracker.Contracts.RequestContracts;
using System.Data.SqlClient;
using Auth.Contracts;

namespace CETrackerDAL.DataAccess;

public interface ICeDataProvider
{
    Task<IEnumerable<DALModels.Experience>> GetExperiencesByYear(int year, int userId, int nationalStandardId, CancellationToken token);
    Task<IEnumerable<DALModels.Experience>> GetExperienceById(int experienceId, CancellationToken token);
    Task<IEnumerable<DALModels.CategoryList>> GetCategoryLists(int year, int userId, CancellationToken token);
    Task<IEnumerable<Location>> GetLocations(CancellationToken token);
    Task<IEnumerable<Unit>> GetUnits(int nationalStandardId, CancellationToken token);
    Task<IEnumerable<DALModels.UserData>> GetUserData(int userId, CancellationToken token);
    Task<int> UpdateExperience(UpdateExperienceRequest request, CancellationToken token);
}

public class CeDataProvider : ICeDataProvider
{
    private readonly IDataConnectionFactory _dataConnectionFactory;

    public CeDataProvider(IDataConnectionFactory dataConnectionFactory)
    {
        _dataConnectionFactory = dataConnectionFactory;
    }

    public Task<IEnumerable<DALModels.Experience>> GetExperiencesByYear(int year, int userId, int nationalStandardId, CancellationToken token) =>
        LoadData<DALModels.Experience, dynamic>(
               "ce.Experiences_S",
            new
            {
                Year = year,
                UserId = userId,
                NationalStandardId = nationalStandardId
            },
            token
        );

    public Task<IEnumerable<DALModels.Experience>> GetExperienceById(int experienceId, CancellationToken token) =>
        LoadData<DALModels.Experience, dynamic>(
               "ce.Experiences_S_By_Id",
            new
            {
                experienceId
            },
            token
        );

    public Task<IEnumerable<DALModels.CategoryList>> GetCategoryLists(int nationalStandardId, int year, CancellationToken token) =>
        LoadData<DALModels.CategoryList, dynamic>(
            "ce.CategoryLists_S"
            ,
            new
            {
                nationalStandardId,
                year
            },
            token
        );

    public Task<IEnumerable<Location>> GetLocations(CancellationToken token) =>
        LoadData<Location, dynamic>(
            "ce.Locations_S",
            new {},
            token
        );

    public Task<IEnumerable<Unit>> GetUnits(int nationalStandardId, CancellationToken token) => 
        LoadData<Unit, dynamic>(
               "ce.Units_S",
               new
               {
                   nationalStandardId
               },
               token);

    public Task<IEnumerable<DALModels.UserData>> GetUserData(int userId, CancellationToken token) =>
        LoadData<DALModels.UserData, dynamic>(
            "ce.UserData_S",
            new
            {
                userId
            },
            token
        );

    public async Task<int> UpdateExperience(UpdateExperienceRequest request, CancellationToken cancellationToken)
    {
        using var txScope = new TransactionScope(TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        using DbConnection connection = _dataConnectionFactory.CeTrackerSqlConnection();
        await connection.OpenAsync();

        try
        {
            var experienceId = await UpdateExperience(request, request.UserId, connection, cancellationToken);

            await UpdateExperienceCategories(experienceId, request, request.UserId, connection, cancellationToken);

            await UpdateExperienceAmounts(experienceId, request, request.UserId, connection, cancellationToken);

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

    internal virtual async Task<int> GetUserId(string username, CancellationToken token)
    {
        try
        {
            var user = await LoadData<User, dynamic>("core.User_By_UserName_S", new { username }, token);
            return user.FirstOrDefault()?.Id ?? 0;
        }
        catch (Exception e)
        {
            // TODO: logging
            throw;
        }
    }

    internal virtual async Task<int> UpdateExperience(
        UpdateExperienceRequest updateExperienceRequest,
        int updateUserId,
        IDbConnection connection,
        CancellationToken cancellationToken)
    {
        var command = new CommandDefinition("ce.Experiences_U_I",
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
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var experienceId = await connection.QuerySingleAsync<int>(command);

        return experienceId;
    }

    internal virtual async Task UpdateExperienceCategories(int experienceId, UpdateExperienceRequest request, int updateUserId, IDbConnection conn, CancellationToken token)
    {
        if (request.Categories.Length == 0)
        {
            return;
        }

        var currentCategories = await LoadData<ExperienceCategory, dynamic>(
            "ce.ExperienceCategory_S",
            new
            {
                experienceId
            }, conn, token);

        var currentCatIds = currentCategories.Select(c => c.CategoryId);

        var categoriesToDelete = currentCategories.Where(cat => !request.Categories.Contains(cat.CategoryId));
        var categoriesToCreate = request.Categories.Where(catId => !currentCatIds.Contains(catId));

        foreach(var oldCategory in categoriesToDelete)
        {
            var command = new CommandDefinition("ce.ExperienceCategory_D", new
            {
                experienceId,
                oldCategory.CategoryId,
                updateUserId
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: token);

            await conn.ExecuteAsync(command);
        }

        foreach(var newCategoryId in categoriesToCreate)
        {
            if (newCategoryId == 0)
                continue;

            var command = new CommandDefinition("ce.ExperienceCategory_I", new
            {
                experienceId,
                CategoryId = newCategoryId,
                updateUserId
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: token);

            await conn.ExecuteAsync(command);
        }

    }

    internal virtual async Task UpdateExperienceAmounts(int experienceId, UpdateExperienceRequest request, int updateUserId, IDbConnection conn, CancellationToken token)
    {
        var currentExperienceAmount = await LoadData<DALModels.ExperienceAmount, dynamic>(
            "ce.ExperienceAmount_S",
            new
            {
                experienceId
            }, conn, token);

        if (currentExperienceAmount == null || currentExperienceAmount.Count() == 0)
        {
            await UpdateExperienceAmount(experienceId, request.TimeSpentParent, updateUserId, conn, token);
            await UpdateExperienceAmount(experienceId, request.TimeSpentChild, updateUserId, conn, token);
        }
        else
        {
            var currentParentAmount = currentExperienceAmount.Where(am => am.ParentUnitId == 0).ToList().FirstOrDefault();
            var currentChildAmount = currentExperienceAmount.Where(am => am.ParentUnitId != 0).ToList().FirstOrDefault();

            if (request.TimeSpentParent.Amount != currentParentAmount.Amount)
                await UpdateExperienceAmount(experienceId, request.TimeSpentParent, updateUserId, conn, token);

            if (request.TimeSpentChild.Amount != currentChildAmount.Amount)
                await UpdateExperienceAmount(experienceId, request.TimeSpentChild, updateUserId, conn, token);
        }
    }

    private async Task UpdateExperienceAmount(int experienceId, ExperienceAmount amount, int updateUserId, IDbConnection conn, CancellationToken token)
    {
        var command = new CommandDefinition("ce.ExperienceAmount_U_I",
            new
            {
                experienceId,
                amount.UnitId,
                amount.Amount,
                updateUserId
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: token);

        await conn.ExecuteAsync(command);
    }

    internal virtual async Task<IEnumerable<T>> LoadData<T, U>(
        string storedProcedure,
        U parameters,
        CancellationToken token,
        string connectionId = "Default"
    )
    {
        using IDbConnection connection = _dataConnectionFactory.CeTrackerSqlConnection();

        var command = new CommandDefinition(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, cancellationToken: token);

        var result = await connection.QueryAsync<T>(command);

        return result;
    }

    /// <summary>
    /// This version takes the connection as an argument instead of establishing
    /// a new connection. This is for use from within a transaction.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="storedProcedure"></param>
    /// <param name="parameters"></param>
    /// <param name="connection"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    internal virtual async Task<IEnumerable<T>> LoadData<T, U>(
    string storedProcedure,
    U parameters,
    IDbConnection connection,
    CancellationToken token
)
    {
        var command = new CommandDefinition(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, cancellationToken: token);

        var result = await connection.QueryAsync<T>(command);

        return result;
    }
}
