namespace CETracker.Contracts.DataContracts;
public class ExperienceCategory
{
    public int? ExperienceCategoryId { get; set; }
    public int? ExperienceId { get; set; }
    public int? CategoryListId { get; set; }
    public int CategoryId { get; set; }
    public string DisplayName { get; set; }
}
