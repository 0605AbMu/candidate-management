using System.ComponentModel.DataAnnotations;

namespace CM.WebApi.Contracts.Candidate;

public class CreateCandidateDto
{
    [MaxLength(100)] public string FirstName { get; set; } = default!;
    [MaxLength(100)] public string LastName { get; set; } = default!;

    [RegularExpression(
        @"^\+998([- ])?(90|91|93|94|95|98|99|33|97|71|77|20|88|33|75)([- ])?(\d{3})([- ])?(\d{2})([- ])?(\d{2})$",
        ErrorMessage = "Wrong phone number format")]
    [MaxLength(100)]
    public string? PhoneNumber { get; set; } = default!;

    [EmailAddress] public string Email { get; set; } = default!;
    public DateTime CallFrom { get; set; }
    public DateTime CallTo { get; set; }
    [Url] public string? LinkedInUrl { get; set; }
    [Url] public string? GithubUrl { get; set; }
    public string Comment { get; set; } = default!;
}
