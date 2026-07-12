using System.Text.Json;
using CETrackerApi.Logic;
using CETrackerDAL.DataAccess;
using DALModels = CETrackerDAL.Models;
using Moq;

namespace CETrackerApi.Tests;

public class CategoryServiceTests
{
    private readonly ICategoryService _categoryService;
    private readonly Mock<ICeDataProvider> _mockDataProvider;

    public CategoryServiceTests()
    {
        _mockDataProvider = new Mock<ICeDataProvider>();

        _categoryService = new CategoryService(_mockDataProvider.Object);
    }

    [Fact]
    public async Task Null_Categories_Returns_Empty()
    {
        List<DALModels.CategoryList> inputData = null;

        _mockDataProvider
            .Setup(m => m.GetCategoryLists(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(inputData);

        var result = await _categoryService.GetCategoryLists(1, 2026, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.False(result.CategoryLists.Any());
    }

    [Fact]
    public async Task No_Categories_Returns_Empty()
    {
        List<DALModels.CategoryList> inputData = [];

        _mockDataProvider
            .Setup(m => m.GetCategoryLists(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(inputData);

        var result = await _categoryService.GetCategoryLists(1, 2026, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.False(result.CategoryLists.Any());
    }

    [Fact]
    public async Task One_List_Single_Category()
    {
        var inputData = TestData.One_List_Single_Category_Input;
        var expectedOutput = TestData.One_List_Single_Category_Expected;

        _mockDataProvider
            .Setup(m => m.GetCategoryLists(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(inputData);

        var result = await _categoryService.GetCategoryLists(1, 2026, TestContext.Current.CancellationToken);

        var expectedJson = JsonSerializer.Serialize(expectedOutput);
        var actualJson = JsonSerializer.Serialize(result);
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public async Task One_List_Multiple_Categories()
    {
        var inputData = TestData.One_List_Multiple_Categories_Input;
        var expectedOutput = TestData.One_List_Multiple_Categories_Expected;

        _mockDataProvider
            .Setup(m => m.GetCategoryLists(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(inputData);

        var result = await _categoryService.GetCategoryLists(1, 2026, TestContext.Current.CancellationToken);

        var expectedJson = JsonSerializer.Serialize(expectedOutput);
        var actualJson = JsonSerializer.Serialize(result);
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public async Task Multiple_Lists_Multiple_Categories()
    {
        var inputData = TestData.One_List_Single_Category_Input;
        inputData.AddRange(TestData.One_List_Multiple_Categories_Input);

        var expectedOutput = TestData.One_List_Single_Category_Expected;
        expectedOutput.CategoryLists = expectedOutput.CategoryLists.Concat(TestData.One_List_Multiple_Categories_Expected.CategoryLists);

        _mockDataProvider
            .Setup(m => m.GetCategoryLists(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(inputData);

        var result = await _categoryService.GetCategoryLists(1, 2026, TestContext.Current.CancellationToken);

        var expectedJson = JsonSerializer.Serialize(expectedOutput);
        var actualJson = JsonSerializer.Serialize(result);
        Assert.Equal(expectedJson, actualJson);
    }
}
