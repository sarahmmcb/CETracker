using DALCategoryList = CETrackerDAL.Models;
using CETracker.Contracts.DataContracts;

namespace CETrackerApi.Logic;

public interface ICategoryService
{
    Task<CategoryListResponse> GetCategoryLists(int nationalStandardId, int year);
}
public class CategoryService: ICategoryService
{
    private readonly ICeDataProvider _ceDataProvider;

    public CategoryService(ICeDataProvider ceDataprovider)
    {
        _ceDataProvider = ceDataprovider;
    }

    public async Task<CategoryListResponse> GetCategoryLists(int nationalStandardId, int year)
    {
        var listData = await _ceDataProvider.GetCategoryLists(nationalStandardId, year);
        var structuredCategoryLists = GetStructuredCategoryLists(listData);

        return new CategoryListResponse
        {
            CategoryLists = structuredCategoryLists
        };
    }

    private IEnumerable<CategoryList> GetStructuredCategoryLists(IEnumerable<DALCategoryList.CategoryList> lists)
    {
        List<CategoryList> categoryLists = new List<CategoryList>();
        foreach(var item in lists)
        {
            var structuredList = categoryLists.FirstOrDefault(m => m.CategoryListId == item.CategoryListId);
            if (structuredList == null)
            {
                structuredList = new CategoryList
                {
                    CategoryListId = item.CategoryListId,
                    Name = item.Name,
                    DisplayQuestion = item.DisplayQuestion,
                    DisplayOrder = item.DisplayOrder,
                    Categories = new List<Category>()
                };
                categoryLists.Add(structuredList);
            }

            var category = new Category
            {
                CategoryId = item.CategoryId,
                CategoryListId = item.CategoryListId,
                Name = item.CategoryName,
                DisplayName = item.DisplayName,
                NationalStandardId = item.NationalStandardId
            };

            structuredList.Categories = structuredList.Categories.Concat(new[] { category });
        }

        return categoryLists;
    } 
}
