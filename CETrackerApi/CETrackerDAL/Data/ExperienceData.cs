using CETrackerDAL.DAL;
using CETrackerDAL.Models;

namespace CETrackerDAL.Data;

public interface IExperienceData
{
	Task<IEnumerable<Experience>> GetExperiencesByYear(int year, int userId, int nationalStandardId);
}

public class ExperienceData : IExperienceData
{
	private readonly IDataAccess _db;

	public ExperienceData(IDataAccess db)
	{
		_db = db;
	}

	public Task<IEnumerable<Experience>> GetExperiencesByYear(int year, int userId, int nationalStandardId) =>
		_db.LoadData<Experience, dynamic>(
			"ce.sp_Select_Experiences_By_User_And_Year",
			new
			{
				Year = year,
				UserId = userId,
				NationalStandardId = nationalStandardId
			}
		);
}
