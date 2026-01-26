namespace CETracker.Contracts.ResponseContracts;

public class CeDataResponse
{
    public string ComplianceStatus { get; set; }
    public IEnumerable<CategoryData> CategoryData { get; set; }

}
public class CategoryData
{
    public int CategoryId { get; set; }
    public string DisplayName { get; set; }
    public decimal Minimum { get; set; }
    public decimal Maximum { get; set; }
    public decimal AmountCompleted { get; set; }
}

