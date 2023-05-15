using CETrackerDAL.DAL;
using CETrackerDAL.Models;

namespace CETrackerDAL.Data;

public interface ICategoryData
{
    Task<IEnumerable<CategoryList>> GetCategoryLists(int nationalStandardId, int year);
}
public class CategoryData : ICategoryData
{
    private readonly IDataAccess _dataAccess;

    public CategoryData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<IEnumerable<CategoryList>> GetCategoryLists(int nationalStandardId, int year) =>
        _dataAccess.LoadData<CategoryList, dynamic>(
            "ce.CategoryList_S"
            ,
            new
            {
                nationalStandardId,
                year
            }
        );
}
