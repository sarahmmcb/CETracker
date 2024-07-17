using CETrackerDAL.DAL;
using CETrackerDAL.Models;

namespace CETrackerDAL.Data;

public interface IExperienceDataProvider
{
	Task<IEnumerable<Experience>> GetExperiencesByYear(int year, int userId, int nationalStandardId);
}

public class ExperienceDataProvider : IExperienceDataProvider
{
	private readonly IDataAccess _db;

	public ExperienceDataProvider(IDataAccess db)
	{
		_db = db;
	}

    public Task<IEnumerable<Experience>> GetExperiencesByYear(int year, int userId, int nationalStandardId) =>
        _db.LoadData<Experience, dynamic>(
               "ce.Experiences_S",
            new
            {
                Year = year,
                UserId = userId,
                NationalStandardId = nationalStandardId
            }
        );
}
