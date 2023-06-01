using DALCategoryList = CETrackerDAL.Models;
using CETracker.Contracts.DataContracts;

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
        List<CategoryList> categoryLists = new List<CategoryList>();
        foreach(var item in lists)
        {
            var structuredList = categoryLists.FirstOrDefault(m => m.CategoryListId == item.CategoryListId);
            if (structuredList == null)
            {
                structuredList = new CategoryList();
                structuredList.CategoryListId = item.CategoryListId;
                structuredList.Name = item.Name;
                structuredList.DisplayQuestion = item.DisplayQuestion;
                structuredList.DisplayOrder = item.DisplayOrder;
                structuredList.Categories = new List<Category>();
                categoryLists.Add(structuredList);
            }

            var category = new Category();
            category.CategoryId = item.CategoryId;
            category.CategoryListId = item.CategoryListId;
            category.Name = item.CategoryName;
            category.DisplayName = item.DisplayName;
            category.NationalStandardId = item.NationalStandardId;

            structuredList.Categories = structuredList.Categories.Concat(new[] { category });
        }

        return categoryLists;
    } 
}
