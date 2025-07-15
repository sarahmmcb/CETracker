using CETracker.Contracts.DataContracts;

namespace CETracker.Contracts.ResponseContracts;
public class UserDataResponse
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public bool CanSignSAO { get; set; }
    public NationalStandard NationalStandard { get; set; }
}

