
using CETracker.Contracts.DataContracts;
using System.Diagnostics.CodeAnalysis;

namespace CETracker.Contracts.RequestContracts;

public class UpdateExperienceRequest
{
    public int? ExperienceId { get; set; }
    public int UserId { get; set; }
    public int LocationId { get; set; }
    public bool CarryForward { get; set; }
    public string ProgramTitle { get; set; }
    public string EventName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public string Notes { get; set; }
    public int[] CategoryIds { get; set; }
    public ExperienceAmount ExperienceAmount { get; set; }
}
