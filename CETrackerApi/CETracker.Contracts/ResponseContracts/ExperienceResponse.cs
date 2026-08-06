using CETracker.Contracts.DataContracts;

namespace CETracker.Contracts.ResponseContracts;
public class ExperienceResponse
{
    public IEnumerable<Experience> Experiences { get; set; } = [];
}
