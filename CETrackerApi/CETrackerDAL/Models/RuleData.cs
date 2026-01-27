namespace CETrackerDAL.Models;
public class RuleData
{
    public int RuleId { get; set; }
	public string RuleName { get; set; }
	public int MainGoal { get; set; }
	public int RuleConditionId { get; set; }
	public int Goal { get; set; }
	public int MaxAmount { get; set; }
	public bool IsDoubleCounted { get; set; }
	public int CategoryId { get; set; }
	public string CategoryName { get; set; }
}
