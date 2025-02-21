namespace CETrackerDAL.Models;
public class ExperienceAmount
{   
    public int ExperienceId { get; set; }
    public int UnitId { get; set; }
    public int ParentUnitId { get; set; }
    public decimal Amount { get; set; }
}
