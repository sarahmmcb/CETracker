namespace CETrackerDAL.Models;

public class Experience
{
    public int ExperienceId { get; set; }
    public int UserId { get; set; }
    public bool CarryForward { get; set; }
    public string ProgramTitle { get; set; }
    public string EventName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public string Notes { get; set; }
    public int ExperienceCategoryId { get; set; }
    public int CategoryId { get; set; }
    public int NationalStandardId { get; set; }
    public int CategoryListId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryDisplayName { get; set; }
    public int ExperienceAmountId { get; set; }
    public int UnitId { get; set; }
    public decimal Amount { get; set; }
    public int LocationId { get; set; }
    public string LocationName { get; set; }
}
