namespace CETracker.Contracts.DataContracts;
public class ExperienceAmount
{
    public int? ExperienceAmountId { get; set; }
    public int? ExperienceId { get; set; }
    public int UnitId { get; set; }
    public decimal Amount { get; set; }
}
