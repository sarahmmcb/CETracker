namespace CETrackerDAL.Models;
public class CeData
{
    public int RuleId { get; set; }
    public string RuleName { get; set; }
    public int Goal { get; set; }
    public int MaxAmount { get; set; }
    public bool IsMainGoal { get; set; }
    public bool IsAdditionalCategory { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string DisplayName { get; set; }
    public decimal CategoryTotal { get; set; }
    public string UnitShortNamePlural { get; set; }
    public string UnitShortNameSingular { get; set; }
}
