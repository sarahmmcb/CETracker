using DALCategoryList = CETrackerDAL.Models;
using CETracker.Contracts.DataContracts;
using CETrackerDAL.Data;

namespace CETrackerApi.Logic;

public interface ICategoryService
{
    Task<IEnumerable<CategoryList>> GetCategoryLists(int nationalStandardId, int year);
}
public class CategoryService: ICategoryService
{
    private readonly ICategoryData _categoryData;

    public CategoryService(ICategoryData categoryData)
    {
        _categoryData = categoryData;
    }

    public async Task<IEnumerable<CategoryList>> GetCategoryLists(int nationalStandardId, int year)
    {
        var listData = await _categoryData.GetCategoryLists(nationalStandardId, year);

        return GetStructuredCategoryLists(listData);
    }

    private IEnumerable<CategoryList> GetStructuredCategoryLists(IEnumerable<DALCategoryList.CategoryList> lists)
    {

    } 
}
