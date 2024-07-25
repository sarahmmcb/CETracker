using CETracker.Contracts.DataContracts;
using CETrackerDAL.Models;

namespace CETrackerApi.Logic;

public interface IExperienceService
{
    Task<IEnumerable<ExperienceResponse>> GetExperiencesByYear(int userId, int year, int nationalStandardId);
}
public class ExperienceService : IExperienceService
{
    private readonly ICeDataProvider _ceDataProvider;

    public ExperienceService(ICeDataProvider ceDataprovider)
    {
        _ceDataProvider = ceDataprovider;
    }

    public async Task<IEnumerable<ExperienceResponse>> GetExperiencesByYear(int year, int userId, int nationalStandardId)
    {
       var experienceData = await _ceDataProvider.GetExperiencesByYear(year, userId, nationalStandardId).ConfigureAwait(false);

       return ConstructExperiences(experienceData);
    }

    public async Task<int> UpdateExperience(UpdateExperienceRequest request)
    {
        await Task.Delay(1000);
        return 0;
    }

    internal virtual IEnumerable<ExperienceResponse> ConstructExperiences(IEnumerable<Experience> experienceData)
    {
        List<ExperienceResponse> experiences = new();
        ExperienceResponse experienceResponse = new();
        var prevId = -1;

        foreach (var experienceRow in experienceData)
        {
            if (experienceRow.ExperienceId != prevId)
            {
                prevId = experienceRow.ExperienceId;

                experienceResponse = new ExperienceResponse
                {
                    ExperienceId = experienceRow.ExperienceId,
                    UserId = experienceRow.UserId,
                    Location = new Location
                    {
                        LocationId = experienceRow.LocationId,
                        Name = experienceRow.LocationName
                    },
                    CarryForward = experienceRow.CarryForward,
                    ProgramTitle = experienceRow.ProgramTitle,
                    EventName = experienceRow.EventName,
                    StartDate = experienceRow.StartDate,
                    EndDate = experienceRow.EndDate,
                    Description = experienceRow.Description,
                    Notes = experienceRow.Notes,
                    Categories = new List<ExperienceCategory>
                    {
                        new() {
                            ExperienceCategoryId = experienceRow.ExperienceCategoryId,
                            ExperienceId = experienceRow.ExperienceId,
                            CategoryId = experienceRow.CategoryId,
                            CategoryListId = experienceRow.CategoryListId,
                            Name = experienceRow.CategoryName,
                            DisplayName = experienceRow.CategoryName,
                        }
                    },
                    Amounts = new List<ExperienceAmount>()
                    {
                        new()
                        {
                            ExperienceAmountId = experienceRow.ExperienceAmountId,
                            UnitId = experienceRow.UnitId,
                            ExperienceId = experienceRow.ExperienceId,
                            Amount = experienceRow.Amount
                        }
                    }
                };

                experiences.Add(experienceResponse);
            }
            else
            {
                if (!experienceResponse.Categories.Any(c => c.CategoryId == experienceRow.CategoryId))
                {
                    experienceResponse.Categories = experienceResponse.Categories.Append(new()
                    {
                        ExperienceCategoryId = experienceRow.ExperienceCategoryId,
                        ExperienceId = experienceRow.ExperienceId,
                        CategoryId = experienceRow.CategoryId,
                        CategoryListId = experienceRow.CategoryListId,
                        Name = experienceRow.CategoryName,
                        DisplayName = experienceRow.CategoryName,
                    });
                }

                if (!experienceResponse.Amounts.Any(am => am.UnitId == experienceRow.UnitId))
                {
                    experienceResponse.Amounts = experienceResponse.Amounts.Append(new()
                    {
                        ExperienceAmountId = experienceRow.ExperienceAmountId,
                        UnitId = experienceRow.UnitId,
                        ExperienceId = experienceRow.ExperienceId,
                        Amount = experienceRow.Amount
                    });
                }
            }
        }

        return experiences;
    }
}
