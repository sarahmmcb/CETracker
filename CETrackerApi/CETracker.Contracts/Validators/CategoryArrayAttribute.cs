using System.ComponentModel.DataAnnotations;
using CETracker.Contracts.Extensions;

namespace CETracker.Contracts.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class CategoryArrayAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        Type? valueType = value?.GetType();
        if (valueType is null || !valueType.IsArrayOf<int>() || (value as int[])!.Length == 0)
        {
            return false;
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be an integer array with at least one element";
    }
}
