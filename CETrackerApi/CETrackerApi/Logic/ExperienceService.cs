using CETracker.Contracts.DataContracts;
using CETrackerApi.Security;
using DALModels = CETrackerDAL.Models;

namespace CETrackerApi.Logic;

public interface IExperienceService
{
    Task<ExperienceResponse> GetExperiencesByYear(int userId, int year, int nationalStandardId, CancellationToken token);
    Task DeleteExperience(int experienceId, CancellationToken token);
    Task<Experience> UpdateExperience(UpdateExperienceRequest request, CancellationToken token);
}
public class ExperienceService(ICeDataProvider ceDataprovider, TokenAccessor tokenAccessor) : IExperienceService
{
    public async Task<ExperienceResponse> GetExperiencesByYear(int year, int userId, int nationalStandardId, CancellationToken token)
    {
       var experienceData = await ceDataprovider.GetExperiencesByYear(year, userId, nationalStandardId, token).ConfigureAwait(false);
       
       if (experienceData == null || !experienceData.Any())
        {
            return new ExperienceResponse();
        }

       return ConstructExperiences(experienceData);
    }

    public async Task DeleteExperience(int experienceId, CancellationToken token)
    {
        var userId = tokenAccessor.GetProperty("UserId");  // TODO: Find a better way to define the TokenAccessor, this has a magic string and have to manually parse
        var parsedUserId = int.TryParse(userId, out var updateUserId);
        if (parsedUserId)
        {
            await ceDataprovider.DeleteExperience(updateUserId, experienceId, token).ConfigureAwait(false);
        }
        else
        {
            // TODO: logging
            throw new ApplicationException("Unable to determine user");
        }
    }

    public async Task<Experience> UpdateExperience(UpdateExperienceRequest request, CancellationToken cancellationToken)
    {
        var experienceId = await ceDataprovider.UpdateExperience(request, cancellationToken);
        var experienceData = await ceDataprovider.GetExperienceById(experienceId, cancellationToken);
        return ConstructExperiences(experienceData).Experiences.ElementAt(0);
    }

    internal virtual ExperienceResponse ConstructExperiences(IEnumerable<DALModels.Experience> experienceData)
    {
        List<Experience> experiences = [];
        Experience experience = new();
        ExperienceResponse experienceResponse = new();
        var prevId = -1;

        foreach (var experienceRow in experienceData)
        {
            if (experienceRow.ExperienceId != prevId)
            {
                prevId = experienceRow.ExperienceId;

                experience = new Experience
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
                    Categories =
                    [
                        new() {
                            ExperienceId = experienceRow.ExperienceId,
                            CategoryId = experienceRow.CategoryId,
                            CategoryListId = experienceRow.CategoryListId,
                            DisplayName = experienceRow.CategoryDisplayName
                        }
                    ],
                    Amounts =
                    [
                        new()
                        {
                            UnitId = experienceRow.UnitId,
                            ExperienceId = experienceRow.ExperienceId,
                            Amount = experienceRow.Amount,
                            UnitLabel = experienceRow.UnitLabel,
                            IsComplianceUnit = experienceRow.IsComplianceUnit,
                        }
                    ]
                };

                experiences.Add(experience);
            }
            else
            {
                if (!experience.Categories.Any(c => c.CategoryId == experienceRow.CategoryId))
                {
                    experience.Categories = experience.Categories.Append(new()
                    {
                        ExperienceId = experienceRow.ExperienceId,
                        CategoryId = experienceRow.CategoryId,
                        CategoryListId = experienceRow.CategoryListId,
                        DisplayName = experienceRow.CategoryDisplayName
                    });
                }

                if (!experience.Amounts.Any(am => am.UnitId == experienceRow.UnitId))
                {
                    experience.Amounts = experience.Amounts.Append(new()
                    {
                        UnitId = experienceRow.UnitId,
                        ExperienceId = experienceRow.ExperienceId,
                        Amount = experienceRow.Amount,
                        UnitLabel = experienceRow.UnitLabel,
                        IsComplianceUnit = experienceRow.IsComplianceUnit
                    });
                }
            }
        }

        experienceResponse.Experiences = experiences;

        return experienceResponse;
    }
}
