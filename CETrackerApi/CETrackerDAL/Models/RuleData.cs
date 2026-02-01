namespace CETrackerDAL.Models;
public class RuleData
{
    public int RuleId { get; set; }
    public string RuleName { get; set; }
    public int Goal { get; set; }
    public int MaxAmount { get; set; }
    public bool IsMainGoal { get; set; }
    public bool IsAdditionalCategory { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}
