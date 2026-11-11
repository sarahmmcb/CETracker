
using System.ComponentModel.DataAnnotations;
using CETracker.Contracts.DataContracts;
using CETracker.Contracts.Validators;

namespace CETracker.Contracts.RequestContracts;

public class UpdateExperienceRequest
{
    public int? ExperienceId { get; set; }
    [Required(ErrorMessage = "UserId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "UserId must be non-negative")]
    public int UserId { get; set; }
    [Required(ErrorMessage = "LocationId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "LocationId must be non-negative")]
    public int LocationId { get; set; }
    [Required(ErrorMessage = "CarryForward is required")]
    public bool CarryForward { get; set; }
    [Required(ErrorMessage = "ProgramTitle is required")]
    [StringLength(200, ErrorMessage = "Program Title cannot be longer than 200 characters")]
    public string ProgramTitle { get; set; }
    [Required(ErrorMessage = "EventName is required")]
    [StringLength(200, ErrorMessage = "Event Name cannot be longer than 200 characters")]
    public string EventName { get; set; }
    [Required(ErrorMessage = "StartDate is required")]
    public DateTime StartDate { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    [Required(ErrorMessage = "Categories are required")]
    [CategoryArray]
    public int[] Categories { get; set; }
    [Required(ErrorMessage = "TimeSpentParent is required")]
    public ExperienceAmount TimeSpentParent { get; set; }
    [Required(ErrorMessage = "TimeSpentChild is required")]
    public ExperienceAmount TimeSpentChild { get; set; }
}
