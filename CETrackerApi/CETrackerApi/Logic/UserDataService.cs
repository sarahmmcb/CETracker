using CETracker.Contracts.DataContracts;

namespace CETrackerApi.Logic;

public interface IUserDataService
{
    Task<UserDataResponse> GetUserData(int userId, CancellationToken token);
}

public class UserDataService : IUserDataService
{
    private readonly ICeDataProvider _ceDataProvider;

    public UserDataService(ICeDataProvider ceDataProvider)
    {
        _ceDataProvider = ceDataProvider;
    }

    public async Task<UserDataResponse> GetUserData(int userId, CancellationToken token)
    {
        var result = await _ceDataProvider.GetUserData(userId, token);

        if (result == null || !result.Any())
        {
            throw new ApplicationException("User data not found");
        }

        var userData = result.FirstOrDefault();

        return new UserDataResponse
        {
            UserId = userData!.UserId,
            Title = userData.Title,
            CanSignSAO = userData.CanSignSAO,
            NationalStandard = new NationalStandard
            {
                NationalStandardId = userData.NationalStandardId,
                OrganizationId = userData.OrganizationId,
                LongName = userData.LongName,
                ShortName = userData.ShortName,
                IsActive = userData.IsActive
            }
        };
    }
}

