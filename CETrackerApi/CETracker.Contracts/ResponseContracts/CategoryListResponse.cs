using CETracker.Contracts.DataContracts;

namespace CETracker.Contracts.ResponseContracts;
public class CategoryListResponse
{
    public IEnumerable<CategoryList> CategoryLists { get; set; }
}
