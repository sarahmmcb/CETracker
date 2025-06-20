using CETracker.Contracts.DataContracts;
using DALModels = CETrackerDAL.Models;

namespace CETrackerApi.Logic;

public interface IExperienceService
{
    Task<IEnumerable<ExperienceResponse>> GetExperiencesByYear(int userId, int year, int nationalStandardId, CancellationToken token);
    Task<ExperienceResponse> UpdateExperience(UpdateExperienceRequest request, string username, CancellationToken token);
}
public class ExperienceService : IExperienceService
{
    private readonly ICeDataProvider _ceDataProvider;

    public ExperienceService(ICeDataProvider ceDataprovider)
    {
        _ceDataProvider = ceDataprovider;
    }

    public async Task<IEnumerable<ExperienceResponse>> GetExperiencesByYear(int year, int userId, int nationalStandardId, CancellationToken token)
    {
       var experienceData = await _ceDataProvider.GetExperiencesByYear(year, userId, nationalStandardId, token).ConfigureAwait(false);
       return ConstructExperiences(experienceData);
    }

    public async Task<ExperienceResponse> UpdateExperience(UpdateExperienceRequest request, string username, CancellationToken cancellationToken)
    {
        var experienceId = await _ceDataProvider.UpdateExperience(request, username, cancellationToken);
        var experienceData = await _ceDataProvider.GetExperienceById(experienceId, cancellationToken);
        return ConstructExperiences(experienceData).ElementAt(0);
    }

    internal virtual IEnumerable<ExperienceResponse> ConstructExperiences(IEnumerable<DALModels.Experience> experienceData)
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
                    Description = experienceRow.Description,
                    Notes = experienceRow.Notes,
                    Categories = new List<ExperienceCategory>
                    {
                        new() {
                            ExperienceId = experienceRow.ExperienceId,
                            CategoryId = experienceRow.CategoryId,
                            CategoryListId = experienceRow.CategoryListId,
                            DisplayName = experienceRow.CategoryDisplayName
                        }
                    },
                    Amounts = new List<ExperienceAmount>()
                    {
                        new()
                        {
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
                        ExperienceId = experienceRow.ExperienceId,
                        CategoryId = experienceRow.CategoryId,
                        CategoryListId = experienceRow.CategoryListId,
                        DisplayName = experienceRow.CategoryDisplayName
                    });
                }

                if (!experienceResponse.Amounts.Any(am => am.UnitId == experienceRow.UnitId))
                {
                    experienceResponse.Amounts = experienceResponse.Amounts.Append(new()
                    {
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
