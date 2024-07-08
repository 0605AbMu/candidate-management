using CM.WebApi.Contracts.Candidate;

namespace CM.WebApi.Services.Interfaces;

public interface ICandidateManager
{
    Task<CandidateViewDto> CreateAsync(CreateCandidateDto dto);
    Task<IReadOnlyList<CandidateViewDto>> GetAll(int skip, int take);
    Task<CandidateViewDto> GetById(long id);
    Task<CandidateViewDto> DeleteById(long id);
}
