using CETracker.Contracts.DataContracts;
using CETrackerDAL.Models;

namespace CETrackerApi.Logic;

public interface IExperienceService
{
    public Task<IEnumerable<ExperienceResponse>> GetExperiencesByYear(int userId, int year, int nationalStandardId);
}
public class ExperienceService : IExperienceService
{
    private readonly IExperienceData _dataAccess;

    public ExperienceService(IExperienceData experienceData)
    {
        _dataAccess = experienceData;
    }

    public async Task<IEnumerable<ExperienceResponse>> GetExperiencesByYear(int userId, int year, int nationalStandardId)
    {
       var experienceData = await _dataAccess.GetExperiencesByYear(year, userId, nationalStandardId).ConfigureAwait(false);

       return ConstructExperiences(experienceData);
    }

    internal virtual IEnumerable<ExperienceResponse> ConstructExperiences(IEnumerable<Experience> experienceData)
    {
        IEnumerable<ExperienceResponse> experiences = new List<ExperienceResponse>();
        ExperienceResponse experienceResponse = new ExperienceResponse();
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
                    Location = new LocationResponse
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
                    Categories = new List<Category>
                    {
                        new Category {
                            CategoryId = experienceRow.CategoryId,
                            NationalStandardId = experienceRow.NationalStandardId,
                            CategoryListId = experienceRow.CategoryListId,
                            Name = experienceRow.CategoryName,
                            DisplayName = experienceRow.CategoryName,
                        }
                    },
                    Amounts = new List<ExperienceAmount>()
                    {
                        new ExperienceAmount
                        {
                            UnitId = experienceRow.UnitId,
                            ExperienceId = experienceRow.ExperienceId,
                            Amount = experienceRow.Amount
                        }
                    }
                };

                experiences.Append(experienceResponse);
            }

            experienceResponse.Categories.Append(new Category
            {
                CategoryId = experienceRow.CategoryId,
                NationalStandardId = experienceRow.NationalStandardId,
                CategoryListId = experienceRow.CategoryListId,
                Name = experienceRow.CategoryName,
                DisplayName = experienceRow.CategoryName,
            });

            experienceResponse.Amounts.Append(new ExperienceAmount
            {
                UnitId = experienceRow.UnitId,
                ExperienceId = experienceRow.ExperienceId,
                Amount = experienceRow.Amount
            }); 
        }

        return experiences;
    }
}
