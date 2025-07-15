namespace CETrackerDAL.Models;
public class UserData
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public bool CanSignSAO { get; set; }
    public int NationalStandardId { get; set; }
    public int OrganizationId { get; set; }
    public string LongName { get; set; }
    public string ShortName { get; set; }
    public bool IsActive { get; set; }
}

