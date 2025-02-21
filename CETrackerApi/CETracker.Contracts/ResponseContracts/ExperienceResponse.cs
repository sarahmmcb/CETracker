using CETracker.Contracts.DataContracts;

namespace CETracker.Contracts.ResponseContracts;
public class ExperienceResponse
{
    public int ExperienceId { get; set; }
    public int UserId { get; set; }
    public Location Location { get; set; }
    public bool CarryForward { get; set; }
    public string ProgramTitle { get; set; }
    public string EventName { get; set; }
    public DateTime StartDate { get; set; }
    public string Description { get; set; }
    public string Notes { get; set; }
    public IEnumerable<ExperienceCategory> Categories { get; set; }
    public IEnumerable<ExperienceAmount> Amounts { get; set; }
}
