using CETracker.Contracts.DataContracts;
using CETrackerApi.Security;
using DALModels = CETrackerDAL.Models;

namespace CETrackerApi.Logic;

public interface IExperienceService
{
    Task<IEnumerable<ExperienceResponse>> GetExperiencesByYear(int userId, int year, int nationalStandardId, CancellationToken token);
    Task DeleteExperience(int experienceId, CancellationToken token);
    Task<ExperienceResponse> UpdateExperience(UpdateExperienceRequest request, CancellationToken token);
}
public class ExperienceService : IExperienceService
{
    private readonly ICeDataProvider _ceDataProvider;
    private readonly TokenAccessor _tokenAccessor;

    public ExperienceService(ICeDataProvider ceDataprovider, TokenAccessor tokenAccessor)
    {
        _ceDataProvider = ceDataprovider;
        _tokenAccessor = tokenAccessor;
    }

    public async Task<IEnumerable<ExperienceResponse>> GetExperiencesByYear(int year, int userId, int nationalStandardId, CancellationToken token)
    {
       var experienceData = await _ceDataProvider.GetExperiencesByYear(year, userId, nationalStandardId, token).ConfigureAwait(false);
       return ConstructExperiences(experienceData);
    }

    public async Task DeleteExperience(int experienceId, CancellationToken token)
    {
        var userId = _tokenAccessor.GetProperty("userId");  // TODO: Find a better way to define the TokenAccessor, this has a magic string and have to manually parse
        var parsedUserId = int.TryParse(userId, out var updateUserId);
        if (parsedUserId)
        {
            await _ceDataProvider.DeleteExperience(updateUserId, experienceId, token).ConfigureAwait(false);
        }
        else
        {
            // TODO: log and throw exception
            return;
        }
    }

    public async Task<ExperienceResponse> UpdateExperience(UpdateExperienceRequest request, CancellationToken cancellationToken)
    {
        var experienceId = await _ceDataProvider.UpdateExperience(request, cancellationToken);
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
                            Amount = experienceRow.Amount,
                            IsComplianceUnit = experienceRow.IsComplianceUnit,
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
