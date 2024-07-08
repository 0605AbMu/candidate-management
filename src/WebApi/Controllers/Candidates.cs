using CM.WebApi.Contracts.Candidate;
using CM.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CM.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class Candidates : ControllerBase
{
    private readonly ICandidateManager _candidateManager;

    public Candidates(ICandidateManager candidateManager)
    {
        this._candidateManager = candidateManager;
    }

    [HttpGet]
    public Task<IReadOnlyList<CandidateViewDto>> GetAll(int skip = 0, int take = 10) =>
        this._candidateManager.GetAll(skip, take);

    [HttpGet("{id:long}")]
    public Task<CandidateViewDto> GetById(long id) =>
        this._candidateManager.GetById(id);

    [HttpPost]
    public Task<CandidateViewDto> Create([FromBody] CreateCandidateDto dto) =>
        this._candidateManager.CreateAsync(dto);

    [HttpPut]
    public Task<CandidateViewDto> Update([FromBody] CreateCandidateDto dto) =>
        this._candidateManager.CreateAsync(dto);

    [HttpDelete("{id:long}")]
    public Task<CandidateViewDto> DeleteById(long id) =>
        this._candidateManager.DeleteById(id);
}
