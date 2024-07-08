namespace CM.WebApi.Contracts.Candidate;

public class CandidateViewDto
{
    public long Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime CallFrom { get; set; }
    public DateTime CallTo { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string Comment { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
