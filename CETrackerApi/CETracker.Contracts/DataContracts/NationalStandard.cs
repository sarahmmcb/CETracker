
namespace CETracker.Contracts.DataContracts;
public class NationalStandard
{
    public int NationalStandardId { get; set; }
    public int OrganizationId { get; set; }
    public string LongName { get; set; }
    public string ShortName { get; set; }
    public bool IsActive { get; set; }

}
