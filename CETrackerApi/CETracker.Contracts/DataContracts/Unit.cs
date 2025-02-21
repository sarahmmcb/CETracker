namespace CETracker.Contracts.DataContracts;

public class Unit
{
    public int UnitId { get; set; }
    public int NationalStandardId { get; set; }
    public int ParentUnitId { get; set; }
    public string UnitSingular { get; set; }
    public string UnitPlural { get; set; }
    public bool IsDisabled { get; set; }
    public string ConversionFormula { get; set; }
}
