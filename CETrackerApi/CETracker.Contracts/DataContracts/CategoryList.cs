
namespace CETracker.Contracts.DataContracts;
public class CategoryList
{
    public int CategoryListId { get; set; }
    public string Name { get; set; }
    public string DisplayQuestion { get; set; }
    public int DisplayOrder { get; set; }
    public IEnumerable<Category> Categories { get; set; }
}
