using System.Text.Json;
using CETrackerApi.Logic;
using CETrackerDAL.DataAccess;
using DALModels = CETrackerDAL.Models;
using Moq;

namespace CETrackerApi.Tests;

public class CEDataServiceTests
{
    private readonly Mock<ICeDataProvider> _mockDataProvider;
    private readonly ICeDataService _ceDataService;

    public CEDataServiceTests()
    {
        _mockDataProvider = new Mock<ICeDataProvider>();

        _ceDataService = new CeDataService(_mockDataProvider.Object);
    }

    [Fact]
    public async Task Null_Data_Throws_Application_Exception()
    {
        List<DALModels.CeData> ceData = null;

        _mockDataProvider
            .Setup(m => m.GetCeData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), TestContext.Current.CancellationToken))
            .ReturnsAsync(ceData);

        var exception = await Assert.ThrowsAsync<ApplicationException>(async () =>
        {
            await _ceDataService.GetUserCeDataByYear(2026, 1, 1, TestContext.Current.CancellationToken);
        });

        Assert.Equal("CE Data or Rule Data Missing", exception.Message);
    }

    [Fact]
    public async Task No_Data_Throws_Application_Exception()
    {
        List<DALModels.CeData> ceData = [];

        _mockDataProvider
            .Setup(m => m.GetCeData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), TestContext.Current.CancellationToken))
            .ReturnsAsync(ceData);

        var exception = await Assert.ThrowsAsync<ApplicationException>(async () =>
        {
            await _ceDataService.GetUserCeDataByYear(2026, 1, 1, TestContext.Current.CancellationToken);
        });

        Assert.Equal("CE Data or Rule Data Missing", exception.Message);
    }

    [Fact]
    public async Task Has_Data_Not_Compliant()
    {
        var ceData = TestData.Has_Data_Not_Compliant_Input;
        var expectedOutput = TestData.Has_Data_Not_Compliant_Output;

        _mockDataProvider
            .Setup(m => m.GetCeData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), TestContext.Current.CancellationToken))
            .ReturnsAsync(ceData);
        
        var result =  await _ceDataService.GetUserCeDataByYear(2026,1,1,TestContext.Current.CancellationToken);

        Assert.Equal("False", result.ComplianceStatus);
        Assert.Equal(expectedOutput.CategoryData.Count(), result.CategoryData.Count());

        var actualJson = JsonSerializer.Serialize(result);
        var expectedJson = JsonSerializer.Serialize(expectedOutput);

        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public async Task Has_Data_Complaint()
    {
        var ceData = TestData.Has_Data_Compliant_Input;
        var expectedOutput = TestData.Has_Data_Compliant_Output;

        _mockDataProvider
            .Setup(m => m.GetCeData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), TestContext.Current.CancellationToken))
            .ReturnsAsync(ceData);

        var result = await _ceDataService.GetUserCeDataByYear(2026, 1, 1, TestContext.Current.CancellationToken);

        Assert.Equal("True", result.ComplianceStatus);
        Assert.Equal(expectedOutput.CategoryData.Count(), result.CategoryData.Count());

        var actualJson = JsonSerializer.Serialize(result);
        var expectedJson = JsonSerializer.Serialize(expectedOutput);

        Assert.Equal(expectedJson, actualJson);
    }

}
