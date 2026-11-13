using System.ComponentModel.DataAnnotations;

namespace CETracker.Contracts.RequestContracts;

public class UpdateUserDataRequest
{
    [Required(ErrorMessage = "UserId is required")]
    public int UserId { get; set; }
    [Required(ErrorMessage = "NationalStandardId is required")]
    public int NationalStandardId { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [StringLength(20, MinimumLength = 0, ErrorMessage = "Title is required, note it can be an empty string")]
    public string Title { get; set; }
    [Required(ErrorMessage = "CanSignSAO is required")]
    public bool CanSignSAO { get; set; }
}
