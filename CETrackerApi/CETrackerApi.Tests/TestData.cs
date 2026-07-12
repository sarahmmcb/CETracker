using DALModels = CETrackerDAL.Models;
using CETracker.Contracts.DataContracts;
using CETracker.Contracts.ResponseContracts;

namespace CETrackerApi.Tests;

internal static class TestData
{
    #region Experiences
    public static List<DALModels.Experience> OneExperienceOneCategory_Input = 
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

    public static List<ExperienceResponse> OneExperienceOneCategory_Expected = [
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
        ];

    public static List<DALModels.Experience> OneExperienceMultipleCategories_Input =
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

    public static List<ExperienceResponse> OneExperienceMultipleCategories_Expected = [
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
        ];

    #endregion

    #region Category Lists

    public static List<DALModels.CategoryList> One_List_Single_Category_Input = 
        [
            new()
            {
                CategoryListId = 10,
                Name = "List A",
                DisplayQuestion = "Choose One",
                DisplayOrder = 1,
                CategoryId = 30,
                CategoryName = "Category A",
                DisplayName = "Category A Display Name",
                NationalStandardId = 200
            }
        ];

    public static CategoryListResponse One_List_Single_Category_Expected = 
        new()
        {
            CategoryLists =
            [
                new()
                {
                    CategoryListId = 10,
                    Name = "List A",
                    DisplayQuestion = "Choose One",
                    DisplayOrder = 1,
                    Categories =
                    [
                        new()
                        {
                            CategoryId = 30,
                            NationalStandardId = 200,
                            CategoryListId = 10,
                            Name = "Category A",
                            DisplayName = "Category A Display Name"
                        }
                    ]

                }
            ]
        };

    public static List<DALModels.CategoryList> One_List_Multiple_Categories_Input = 
        [
            new()
            {
                CategoryListId = 11,
                Name = "List B",
                DisplayQuestion = "Choose One Or Many",
                DisplayOrder = 2,
                CategoryId = 40,
                CategoryName = "Category B",
                DisplayName = "Category B Display Name",
                NationalStandardId = 200
            },
            new()
            {
                CategoryListId = 11,
                Name = "List B",
                DisplayQuestion = "Choose One Or Many",
                DisplayOrder = 2,
                CategoryId = 50,
                CategoryName = "Category C",
                DisplayName = "Category C Display Name",
                NationalStandardId = 200
            }
        ];

    public static CategoryListResponse One_List_Multiple_Categories_Expected = 
        new()
        {
            CategoryLists =
            [
                new()
                {
                    CategoryListId = 11,
                    Name = "List B",
                    DisplayQuestion = "Choose One Or Many",
                    DisplayOrder = 2,
                    Categories =
                    [
                        new()
                        {
                            CategoryId = 40,
                            NationalStandardId = 200,
                            CategoryListId = 11,
                            Name = "Category B",
                            DisplayName = "Category B Display Name"
                        },
                        new()
                        {
                            CategoryId = 50,
                            NationalStandardId = 200,
                            CategoryListId = 11,
                            Name = "Category C",
                            DisplayName = "Category C Display Name"
                        }
                    ]

                }
            ]
        };

    #endregion

    #region CEData

    public static List<DALModels.CeData> Has_Data_Not_Compliant_Input = [
            new() {
                RuleId = 1,
                RuleName = "Total CE",
                Goal = 30,
                MaxAmount = 0,
                IsMainGoal = true,
                IsAdditionalCategory = false
            },
            new() {
                RuleId = 2,
                RuleName = "Professionalism",
                Goal = 3,
                MaxAmount = 0,
                IsMainGoal = false,
                IsAdditionalCategory = false,
                CategoryId = 2,
                DisplayName = "Professionalism",
                CategoryTotal = 4.0m,
                UnitShortNamePlural = "Hrs.",
                UnitShortNameSingular = "Hr."
            },
            new() {
                RuleId = 3,
                RuleName = "Bias",
                Goal = 1,
                MaxAmount = 0,
                IsMainGoal = false,
                IsAdditionalCategory = true,
                CategoryId = 3,
                DisplayName = "Bias",
                CategoryTotal = 0.6m,
                UnitShortNamePlural = "Hrs.",
                UnitShortNameSingular = "Hr."
            },
            new() {
                RuleId = 4,
                RuleName = "General Business",
                Goal = 0,
                MaxAmount = 3,
                IsMainGoal = false,
                IsAdditionalCategory = false,
                CategoryId = 4,
                DisplayName = "General Business",
                CategoryTotal = 1.2m,
                UnitShortNamePlural = "Hrs.",
                UnitShortNameSingular = "Hr."
            },
            new() {
                RuleId = 6,
                RuleName = "Organized",
                Goal = 6,
                MaxAmount = 0,
                IsMainGoal = false,
                IsAdditionalCategory = true,
                CategoryId = 6,
                DisplayName = "Organized",
                CategoryTotal = 2.8m,
                UnitShortNamePlural = "Hrs.",
                UnitShortNameSingular = "Hr."
            },
            new() {
                RuleId = 0,
                RuleName = "Other Relevant",
                Goal = 0,
                MaxAmount = 0,
                IsMainGoal = false,
                IsAdditionalCategory = false,
                CategoryId = 5,
                DisplayName = "Other Relevant",
                CategoryTotal = 0.6m,
                UnitShortNamePlural = "Hrs.",
                UnitShortNameSingular = "Hr."
            }

        ];

    public static CeDataResponse Has_Data_Not_Compliant_Output = new()
    {
        ComplianceStatus = "False",
        UnitShortNamePlural = "Hrs.",
        UnitShortNameSingular = "Hr.",

        CategoryData = [
            new() {
                CategoryId = 2,
                DisplayName = "Professionalism",
                Minimum = 3m,
                Maximum = 0m,
                AmountCompleted = 4.0m
            },
            new() {
                CategoryId = 3,
                DisplayName = "Bias",
                Minimum = 1m,
                Maximum = 0m,
                AmountCompleted = 0.6m
            },
            new() {
                CategoryId = 4,
                DisplayName = "General Business",
                Minimum = 0m,
                Maximum = 3m,
                AmountCompleted = 1.2m
            },
            new() {
                CategoryId = 6,
                DisplayName = "Organized",
                Minimum = 6m,
                Maximum = 0m,
                AmountCompleted = 2.8m
            },
            new() {
                CategoryId = 5,
                DisplayName = "Other Relevant",
                Minimum = 0m,
                Maximum = 0m,
                AmountCompleted = 0.6m
            },
            new() {
                CategoryId = 0,
                DisplayName = "Total CE",
                Minimum = 30m,
                Maximum = 0m,
                AmountCompleted = 5.8m
            }
        ]
    };

    public static List<DALModels.CeData> Has_Data_Compliant_Input = [
        new() {
                RuleId = 1,
                RuleName = "Total CE",
                Goal = 30,
                MaxAmount = 0,
                IsMainGoal = true,
                IsAdditionalCategory = false
            },
            new() {
                RuleId = 2,
                RuleName = "Professionalism",
                Goal = 3,
                MaxAmount = 0,
                IsMainGoal = false,
                IsAdditionalCategory = false,
                CategoryId = 2,
                DisplayName = "Professionalism",
                CategoryTotal = 6.0m,
                UnitShortNamePlural = "Hrs.",
                UnitShortNameSingular = "Hr."
            },
            new() {
                RuleId = 3,
                RuleName = "Bias",
                Goal = 1,
                MaxAmount = 0,
                IsMainGoal = false,
                IsAdditionalCategory = true,
                CategoryId = 3,
                DisplayName = "Bias",
                CategoryTotal = 1.0m,
                UnitShortNamePlural = "Hrs.",
                UnitShortNameSingular = "Hr."
            },
            new() {
                RuleId = 4,
                RuleName = "General Business",
                Goal = 0,
                MaxAmount = 3,
                IsMainGoal = false,
                IsAdditionalCategory = false,
                CategoryId = 4,
                DisplayName = "General Business",
                CategoryTotal = 4.0m,
                UnitShortNamePlural = "Hrs.",
                UnitShortNameSingular = "Hr."
            },
            new() {
                RuleId = 6,
                RuleName = "Organized",
                Goal = 6,
                MaxAmount = 0,
                IsMainGoal = false,
                IsAdditionalCategory = true,
                CategoryId = 6,
                DisplayName = "Organized",
                CategoryTotal = 10.0m,
                UnitShortNamePlural = "Hrs.",
                UnitShortNameSingular = "Hr."
            },
            new() {
                RuleId = 0,
                RuleName = "Other Relevant",
                Goal = 0,
                MaxAmount = 0,
                IsMainGoal = false,
                IsAdditionalCategory = false,
                CategoryId = 5,
                DisplayName = "Other Relevant",
                CategoryTotal = 50.0m,
                UnitShortNamePlural = "Hrs.",
                UnitShortNameSingular = "Hr."
            }

    ];

    public static CeDataResponse Has_Data_Compliant_Output = new()
    {
        ComplianceStatus = "True",
        UnitShortNamePlural = "Hrs.",
        UnitShortNameSingular = "Hr.",

        CategoryData = [
            new() {
                CategoryId = 2,
                DisplayName = "Professionalism",
                Minimum = 3m,
                Maximum = 0m,
                AmountCompleted = 6.0m
            },
            new() {
                CategoryId = 3,
                DisplayName = "Bias",
                Minimum = 1m,
                Maximum = 0m,
                AmountCompleted = 1.0m
            },
            new() {
                CategoryId = 4,
                DisplayName = "General Business",
                Minimum = 0m,
                Maximum = 3m,
                AmountCompleted = 4.0m
            },
            new() {
                CategoryId = 6,
                DisplayName = "Organized",
                Minimum = 6m,
                Maximum = 0m,
                AmountCompleted = 10.0m
            },
            new() {
                CategoryId = 5,
                DisplayName = "Other Relevant",
                Minimum = 0m,
                Maximum = 0m,
                AmountCompleted = 50.0m
            },
            new() {
                CategoryId = 0,
                DisplayName = "Total CE",
                Minimum = 30m,
                Maximum = 0m,
                AmountCompleted = 59.0m
            }
        ]
    };

    #endregion
}
