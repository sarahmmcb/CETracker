using CETrackerDAL.DAL;
using CETracker.Contracts.ResponseContracts;

namespace CETrackerDAL.Data;

public interface IExperienceData
{
	Task<IEnumerable<ExperienceResponse>> GetExperiencesByYear(int year, int userId, int nationalStandardId);
}

public class ExperienceData : IExperienceData
{
	private readonly IDataAccess _db;

	public ExperienceData(IDataAccess db)
	{
		_db = db;
	}

	public Task<IEnumerable<ExperienceResponse>> GetExperiencesByYear(int year, int userId, int nationalStandardId) =>
		_db.LoadData<ExperienceResponse, dynamic>(
			"ce.sp_Select_Experiences_By_User_And_Year",
			new
			{
				Year = year,
				UserId = userId,
				NationalStandardId = nationalStandardId
			}
		);
}
