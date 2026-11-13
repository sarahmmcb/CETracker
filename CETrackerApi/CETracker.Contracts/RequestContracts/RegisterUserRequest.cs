
namespace CETracker.Contracts.RequestContracts;

public class RegisterUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int NationalStandardId { get; set;  }
    public string Title { get; set; }
    public bool CanSignSAO { get; set; }
}
