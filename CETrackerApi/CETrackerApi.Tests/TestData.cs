using DALModels = CETrackerDAL.Models;
using CETracker.Contracts.DataContracts;
using CETracker.Contracts.ResponseContracts;

namespace CETrackerApi.Tests;

internal static class TestData
{
    public static List<DALModels.Experience> OneExperienceOneCategory_Input()
    {
        return
        [
            new()
            {
                ExperienceId = 1,
                UserId = 2,
                NationalStandardId = 3,
                LocationId = 4,
                LocationName = "Location A",
                CarryForward = false,
                ProgramTitle = "Program A",
                EventName = "Event A",
                StartDate = new DateTime(2023, 1, 1),
                Description = "Description A",
                Notes = "Notes A",
                ExperienceCategoryId = 5,
                CategoryId = 6,
                CategoryListId = 7,
                CategoryName = "Category A",
                CategoryDisplayName = "Category Display A",
                ExperienceAmountId = 8,
                UnitId = 9,
                Amount = 10.0m,
                IsComplianceUnit = true,
            },
            new()
            {
                ExperienceId = 1,
                UserId = 2,
                NationalStandardId = 3,
                LocationId = 4,
                LocationName = "Location A",
                CarryForward = false,
                ProgramTitle = "Program A",
                EventName = "Event A",
                StartDate = new DateTime(2023, 1, 1),
                Description = "Description A",
                Notes = "Notes A",
                ExperienceCategoryId = 5,
                CategoryId = 6,
                CategoryListId = 7,
                CategoryName = "Category A",
                CategoryDisplayName = "Category Display A",
                ExperienceAmountId = 88,
                UnitId = 99,
                Amount = 20.0m,
                IsComplianceUnit = false,
            }
        ];
    }

    public static List<ExperienceResponse> OneExperienceOneCategory_Expected()
    {
        return new List<ExperienceResponse>
        {
            new ExperienceResponse
            {
                ExperienceId = 1,
                UserId = 2,
                Location = new Location
                {
                    LocationId = 4,
                    Name = "Location A"
                },
                CarryForward = false,
                ProgramTitle = "Program A",
                EventName = "Event A",
                StartDate = new DateTime(2023, 1, 1),
                Description = "Description A",
                Notes = "Notes A",
                Categories =
                [
                    new() {
                        ExperienceId = 1,
                        CategoryId = 6,
                        CategoryListId = 7,
                        DisplayName = "Category Display A"
                    }
                ],
                Amounts =
                [
                    new()
                    {
                        UnitId = 9,
                        ExperienceId = 1,
                        Amount = 10.0m,
                        IsComplianceUnit = true
                    },
                    new()
                    {
                        UnitId = 99,
                        ExperienceId = 1,
                        Amount = 20.0m,
                        IsComplianceUnit = false
                    }
                ]
            }
        };
    }

    public static List<DALModels.Experience> OneExperienceMultipleCategories_Input()
    {
        return
        [
            new()
            {
                ExperienceId = 100,
                UserId = 2,
                NationalStandardId = 3,
                LocationId = 4,
                LocationName = "Location A",
                CarryForward = false,
                ProgramTitle = "Program A",
                EventName = "Event A",
                StartDate = new DateTime(2023, 1, 1),
                Description = "Description A",
                Notes = "Notes A",
                ExperienceCategoryId = 5,
                CategoryId = 6,
                CategoryListId = 7,
                CategoryName = "Category A",
                CategoryDisplayName = "Category Display A",
                ExperienceAmountId = 8,
                UnitId = 9,
                Amount = 10.0m,
                IsComplianceUnit = true,
            },
            new()
            {
                ExperienceId = 100,
                UserId = 2,
                NationalStandardId = 3,
                LocationId = 4,
                LocationName = "Location A",
                CarryForward = false,
                ProgramTitle = "Program A",
                EventName = "Event A",
                StartDate = new DateTime(2023, 1, 1),
                Description = "Description A",
                Notes = "Notes A",
                ExperienceCategoryId = 5,
                CategoryId = 6,
                CategoryListId = 7,
                CategoryName = "Category A",
                CategoryDisplayName = "Category Display A",
                ExperienceAmountId = 88,
                UnitId = 99,
                Amount = 20.0m,
                IsComplianceUnit = false,
            },
            new()
            {
                ExperienceId = 100,
                UserId = 2,
                NationalStandardId = 3,
                LocationId = 4,
                LocationName = "Location A",
                CarryForward = false,
                ProgramTitle = "Program A",
                EventName = "Event A",
                StartDate = new DateTime(2023, 1, 1),
                Description = "Description A",
                Notes = "Notes A",
                ExperienceCategoryId = 55,
                CategoryId = 66,
                CategoryListId = 77,
                CategoryName = "Category B",
                CategoryDisplayName = "Category Display B",
                ExperienceAmountId = 8,
                UnitId = 9,
                Amount = 10.0m,
                IsComplianceUnit = true,
            },
            new()
            {
                ExperienceId = 100,
                UserId = 2,
                NationalStandardId = 3,
                LocationId = 4,
                LocationName = "Location A",
                CarryForward = false,
                ProgramTitle = "Program A",
                EventName = "Event A",
                StartDate = new DateTime(2023, 1, 1),
                Description = "Description A",
                Notes = "Notes A",
                ExperienceCategoryId = 55,
                CategoryId = 66,
                CategoryListId = 77,
                CategoryName = "Category B",
                CategoryDisplayName = "Category Display B",
                ExperienceAmountId = 88,
                UnitId = 99,
                Amount = 20.0m,
                IsComplianceUnit = false,
            }
        ];
    }

    public static List<ExperienceResponse> OneExperienceMultipleCategories_Expected()
    {
        return new List<ExperienceResponse>
        {
            new ExperienceResponse
            {
                ExperienceId = 100,
                UserId = 2,
                Location = new Location
                {
                    LocationId = 4,
                    Name = "Location A"
                },
                CarryForward = false,
                ProgramTitle = "Program A",
                EventName = "Event A",
                StartDate = new DateTime(2023, 1, 1),
                Description = "Description A",
                Notes = "Notes A",
                Categories =
                [
                    new() {
                        ExperienceId = 100,
                        CategoryId = 6,
                        CategoryListId = 7,
                        DisplayName = "Category Display A"
                    },
                    new() {
                        ExperienceId = 100,
                        CategoryId = 66,
                        CategoryListId = 77,
                        DisplayName = "Category Display B"
                    }
                ],
                Amounts =
                [
                    new()
                    {
                        UnitId = 9,
                        ExperienceId = 100,
                        Amount = 10.0m,
                        IsComplianceUnit = true
                    },
                    new()
                    {
                        UnitId = 99,
                        ExperienceId = 100,
                        Amount = 20.0m,
                        IsComplianceUnit = false
                    }
                ]
            }
        };
    }
}
