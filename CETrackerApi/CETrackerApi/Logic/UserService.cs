using System.Net;
using Auth.Contracts;
using Auth.Contracts.RequestContracts;

namespace CETrackerApi.Logic;

public interface IUserService
{
    public Task<int> RegisterUser(RegisterUserRequest request, CancellationToken token);
    public Task UpdateUserData(UpdateUserDataRequest request, int updateUserId, CancellationToken token);
}

public class UserService(IHttpClientFactory _httpClientFactory, ICeDataProvider _ceDataProvider) : IUserService
{
    public async Task<int> RegisterUser(RegisterUserRequest registerUserRequest, CancellationToken token)
    {
        var newUserId = await CreateUser(registerUserRequest, token);

        await CreateUserData(registerUserRequest, newUserId, token);

        return await Task.FromResult(newUserId);
    }

    public async Task UpdateUserData(UpdateUserDataRequest request, int updateUserId, CancellationToken token)
    {
        await _ceDataProvider.UpdateUserData(request, updateUserId, token);
    }

    private async Task<int> CreateUser(RegisterUserRequest registerUserRequest, CancellationToken token)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("https://localhost:7143/api"); // TODO: Pull from config, need to bind config to object

        var createUserRequest = new CreateUserRequest
        {
            FirstName = registerUserRequest.FirstName,
            LastName = registerUserRequest.LastName,
            Email = registerUserRequest.Email,
            Username = registerUserRequest.Email,
            Password = registerUserRequest.Password,
            AccountStatus = AccountStatus.Active
        };

        var response = await httpClient.PostAsJsonAsync("api/User/register", createUserRequest, token);
        var responseContent = await response.Content.ReadAsStringAsync(token);
        Console.WriteLine(responseContent);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            Console.WriteLine($"Non-Success status code returned from AuthApi: {responseContent}");
            throw new ApplicationException(responseContent);
        }

        var parsedContent = int.TryParse(responseContent, out int result);

        if (parsedContent)
        {
            return result;
        }
        else
        {
            Console.WriteLine($"Error parsing new user id, returned result: {result}");
            throw new ApplicationException("An error occurred, please try again later.");
        }
    }

    private async Task CreateUserData(RegisterUserRequest request, int newUserId, CancellationToken token)
    {
        var updateRequest = new UpdateUserDataRequest
        {
            UserId = newUserId,
            NationalStandardId = request.NationalStandardId,
            Title = request.Title,
            CanSignSAO = request.CanSignSAO
        };

        try
        {
            await UpdateUserData(updateRequest, newUserId, token);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"User created, Id: {newUserId}, but user data failed to save: {ex.Message}");
            throw new ApplicationException("User was created, but some user data failed to save. Please try again later.");
        }
    }

}
