namespace CETrackerDAL.Models;

public class CategoryList
{
    public int CategoryListId { get; set; }
    public string Name { get; set; }
    public string DisplayQuestion { get; set; }
    public int DisplayOrder { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string DisplayName { get; set; }
    public int NationalStandardId { get; set; }
}
