using CETrackerApi.Logic;
using CETrackerApi.Security;
using CETrackerDAL.DataAccess;
using Moq;
using System.Text.Json;

namespace CETrackerApi.Tests;

public class ExperienceServiceTests
{
    private readonly IExperienceService _experienceService;
    private readonly Mock<ICeDataProvider> _mockDataProvider;
    private readonly Mock<TokenAccessor> _mockTokenAccessor;

    public ExperienceServiceTests()
    {
        _mockDataProvider = new Mock<ICeDataProvider>();
        _mockTokenAccessor = new Mock<TokenAccessor>();

        _experienceService = new ExperienceService(_mockDataProvider.Object, _mockTokenAccessor.Object);
    }
    

    [Fact]
    public async Task One_Experience_One_Category()
    {
        var inputData = TestData.OneExperienceOneCategory_Input();

        var expectedOutput = TestData.OneExperienceOneCategory_Expected();

        _mockDataProvider
            .Setup(m => m.GetExperiencesByYear(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(inputData);

        var result = await _experienceService.GetExperiencesByYear(1, 2026, 1, TestContext.Current.CancellationToken);

        var actualList = result.ToList();

        Assert.Equal(expectedOutput.Count, actualList.Count);

        for (int i = 0; i < expectedOutput.Count; i++)
        {
            var expectedJson = JsonSerializer.Serialize(expectedOutput[i]);
            var actualJson = JsonSerializer.Serialize(actualList[i]);
            Assert.Equal(expectedJson, actualJson);
        }
    }

    [Fact]
    public async Task One_Experience_Multiple_Categories()
    {
        var inputData = TestData.OneExperienceMultipleCategories_Input();

        var expectedOutput = TestData.OneExperienceMultipleCategories_Expected();

        _mockDataProvider
            .Setup(m => m.GetExperiencesByYear(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(inputData);

        var result = await _experienceService.GetExperiencesByYear(1, 2026, 1, TestContext.Current.CancellationToken);

        var actualList = result.ToList();

        Assert.Equal(expectedOutput.Count, actualList.Count);

        for (int i = 0; i < expectedOutput.Count; i++)
        {
            var expectedJson = JsonSerializer.Serialize(expectedOutput[i]);
            var actualJson = JsonSerializer.Serialize(actualList[i]);
            Assert.Equal(expectedJson, actualJson);
        }
    }

    [Fact]
    public async Task Multiple_Experiences()
    {
        var inputData = TestData.OneExperienceOneCategory_Input();
        inputData.AddRange(TestData.OneExperienceMultipleCategories_Input());

        var expectedOutput = TestData.OneExperienceOneCategory_Expected();
        expectedOutput.AddRange(TestData.OneExperienceMultipleCategories_Expected());

        _mockDataProvider
            .Setup(m => m.GetExperiencesByYear(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(inputData);

        var result = await _experienceService.GetExperiencesByYear(1, 2026, 1, TestContext.Current.CancellationToken);

        var actualList = result.ToList();

        Assert.Equal(expectedOutput.Count, actualList.Count);

        for (int i = 0; i < expectedOutput.Count; i++)
        {
            var expectedJson = JsonSerializer.Serialize(expectedOutput[i]);
            var actualJson = JsonSerializer.Serialize(actualList[i]);
            Assert.Equal(expectedJson, actualJson);
        }
    }
}
